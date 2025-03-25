using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace DungeonSlime
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // 3D objects
        private VertexBuffer _sphereVertexBuffer;
        private IndexBuffer _sphereIndexBuffer;
        private BasicEffect _leftSphereEffect;
        private BasicEffect _rightSphereEffect;
        private Texture2D _cobblestoneTexture;
        private int _primitiveCount;

        // Camera/view settings
        private Matrix _leftWorld = Matrix.Identity;
        private Matrix _rightWorld = Matrix.Identity;
        private Matrix _view;
        private Matrix _projection;

        // Positions for the spheres - adjusted for better spacing
        private Vector3 _leftSpherePosition = new Vector3(-4, 0, 0);
        private Vector3 _rightSpherePosition = new Vector3(4, 0, 0);

        // Sphere radius - increased to approx half screen width
        private float _sphereRadius = 2.5f;

        // Rotation angles for the spheres
        private float _leftSphereRotation = 0.0f;
        private float _rightSphereRotation = 0.0f;

        // Auto-rotation settings
        private bool _autoRotate = false;
        private float _autoRotationSpeed = 0.0025f;

        // Input state handling
        private KeyboardState _previousKeyboardState;

        // For screenshot with transparency
        private RenderTarget2D _renderTarget;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set window size to 1280x720
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            // Setup camera - moved farther back to view the larger spheres
            _view = Matrix.CreateLookAt(
                new Vector3(0, 0, 15), // Camera position moved back
                Vector3.Zero,           // Look target
                Vector3.Up             // Up direction
            );

            // Setup projection
            _projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),  // Field of view
                _graphics.GraphicsDevice.Viewport.AspectRatio,
                0.1f,   // Near plane
                100f    // Far plane
            );

            // Initialize keyboard state
            _previousKeyboardState = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load cobblestone texture
            _cobblestoneTexture = Content.Load<Texture2D>("cobblestone");

            // Create a sphere with larger radius
            CreateSphere(GraphicsDevice, _sphereRadius, 32);

            // Setup effects for both spheres
            _leftSphereEffect = new BasicEffect(GraphicsDevice);
            _leftSphereEffect.TextureEnabled = true;
            _leftSphereEffect.Texture = _cobblestoneTexture;
            _leftSphereEffect.LightingEnabled = false;
            // _leftSphereEffect.EnableDefaultLighting();

            _rightSphereEffect = new BasicEffect(GraphicsDevice);
            _rightSphereEffect.TextureEnabled = true;
            _rightSphereEffect.Texture = _cobblestoneTexture;
            _rightSphereEffect.LightingEnabled = false;
            // _rightSphereEffect.EnableDefaultLighting();

            // Create render target for transparent screenshots
            _renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24,
                0,
                RenderTargetUsage.PreserveContents);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get the current keyboard state
            KeyboardState currentKeyboardState = Keyboard.GetState();

            // Check for Enter key to toggle auto-rotation
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                _autoRotate = !_autoRotate;
            }

            // Apply auto-rotation if enabled
            if (_autoRotate)
            {
                _leftSphereRotation += _autoRotationSpeed;
                _rightSphereRotation += _autoRotationSpeed;
            }
            else
            {
                // Manual rotation for right sphere only when auto-rotation is disabled
                if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    _rightSphereRotation -= 0.02f; // Rotate counter-clockwise
                }
                if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    _rightSphereRotation += 0.02f; // Rotate clockwise
                }
            }

            // Check for space key to take a screenshot
            if (currentKeyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space))
            {
                SaveTransparentScreenshot();
            }

            // Update the world matrices
            _leftWorld = Matrix.CreateRotationY(_leftSphereRotation);
            _rightWorld = Matrix.CreateRotationY(_rightSphereRotation);

            // Store current keyboard state for next frame
            _previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Set the render target
            GraphicsDevice.SetRenderTarget(_renderTarget);

            // Clear with transparent background
            GraphicsDevice.Clear(Color.Transparent);

            // Set render states
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.CullCounterClockwiseFace };

            // Set vertex and index buffers
            GraphicsDevice.SetVertexBuffer(_sphereVertexBuffer);
            GraphicsDevice.Indices = _sphereIndexBuffer;

            // Draw the left sphere with Point filtering
            DrawSphere(_leftSphereEffect, _leftSpherePosition, SamplerState.PointClamp, _leftWorld);

            // Draw the right sphere with Anisotropic filtering
            DrawSphere(_rightSphereEffect, _rightSpherePosition, SamplerState.AnisotropicClamp, _rightWorld);

            // Reset render target to the back buffer for display
            GraphicsDevice.SetRenderTarget(null);

            // Clear the back buffer with blue background
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the render target to the screen
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawSphere(BasicEffect effect, Vector3 position, SamplerState samplerState, Matrix worldMatrix)
        {
            // Set the world matrix with position
            effect.World = worldMatrix * Matrix.CreateTranslation(position);
            effect.View = _view;
            effect.Projection = _projection;

            // Set sampler state for texture filtering
            GraphicsDevice.SamplerStates[0] = samplerState;

            // Apply the effect and draw the sphere
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _primitiveCount);
            }
        }

        private void CreateSphere(GraphicsDevice graphicsDevice, float radius, int tessellation)
        {
            int horizontalSegments = tessellation;
            int verticalSegments = tessellation;

            // Create vertex and index lists
            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
            List<short> indices = new List<short>();

            // Create rings of vertices at progressively higher latitudes
            for (int i = 0; i <= verticalSegments; i++)
            {
                float v = 1.0f - (float)i / verticalSegments;

                float latitude = (i * MathHelper.Pi / verticalSegments) - MathHelper.PiOver2;
                float dy = (float)Math.Sin(latitude);
                float dxz = (float)Math.Cos(latitude);

                // Create a single ring of vertices at this latitude
                for (int j = 0; j <= horizontalSegments; j++)
                {
                    float u = (float)j / horizontalSegments;

                    float longitude = j * MathHelper.TwoPi / horizontalSegments;
                    float dx = (float)Math.Cos(longitude) * dxz;
                    float dz = (float)Math.Sin(longitude) * dxz;

                    Vector3 position = new Vector3(dx, dy, dz) * radius;
                    Vector3 normal = Vector3.Normalize(position);
                    Vector2 textureCoordinate = new Vector2(u, v);

                    vertices.Add(new VertexPositionNormalTexture(position, normal, textureCoordinate));
                }
            }

            // Create indices for each face
            for (int i = 0; i < verticalSegments; i++)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    int current = i * (horizontalSegments + 1) + j;
                    int next = current + 1;
                    int nextRow = (i + 1) * (horizontalSegments + 1) + j;
                    int nextRowNext = nextRow + 1;

                    indices.Add((short)current);
                    indices.Add((short)nextRow);
                    indices.Add((short)next);

                    indices.Add((short)next);
                    indices.Add((short)nextRow);
                    indices.Add((short)nextRowNext);
                }
            }

            // Create the vertex buffer
            _sphereVertexBuffer = new VertexBuffer(
                graphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                vertices.Count,
                BufferUsage.WriteOnly);
            _sphereVertexBuffer.SetData(vertices.ToArray());

            // Create the index buffer
            _sphereIndexBuffer = new IndexBuffer(
                graphicsDevice,
                IndexElementSize.SixteenBits,
                indices.Count,
                BufferUsage.WriteOnly);
            _sphereIndexBuffer.SetData(indices.ToArray());

            // Set the primitive count (number of triangles)
            _primitiveCount = indices.Count / 3;
        }

        private void SaveTransparentScreenshot()
        {
            // Create a directory for screenshots if it doesn't exist
            string directory = "Screenshots";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Generate a unique filename with timestamp
            string filename = Path.Combine(directory, $"Spheres_{DateTime.Now:yyyyMMdd_HHmmss}.png");

            // Get the texture data from the render target
            Color[] data = new Color[_renderTarget.Width * _renderTarget.Height];
            _renderTarget.GetData(data);

            // Save the texture as a PNG file
            using (FileStream stream = File.OpenWrite(filename))
            {
                _renderTarget.SaveAsPng(stream, _renderTarget.Width, _renderTarget.Height);
            }

            Console.WriteLine($"Screenshot saved to {filename}");
        }
    }
}
