using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Audio
{
    /// <summary>
    /// Interface for the game's audio manager that handles sound effects and music.
    /// </summary>
    public interface IAudioManager
    {
        /// <summary>
        /// Gets whether audio is currently muted.
        /// </summary>
        bool IsMuted { get; }

        /// <summary>
        /// Adds a sound effect to the audio manager.
        /// </summary>
        /// <param name="assetName">The content asset name of the sound effect.</param>
        void AddSoundEffect(string assetName);

        /// <summary>
        /// Adds a song to the audio manager.
        /// </summary>
        /// <param name="assetName">The content asset name of the song.</param>
        void AddSong(string assetName);

        /// <summary>
        /// Plays a sound effect.
        /// </summary>
        /// <param name="assetName">The asset name of the sound effect to play.</param>
        /// <returns>The sound effect instance that's playing.</returns>
        Microsoft.Xna.Framework.Audio.SoundEffectInstance PlaySoundEffect(string assetName);

        /// <summary>
        /// Plays a sound effect with custom parameters.
        /// </summary>
        /// <param name="assetName">The asset name of the sound effect to play.</param>
        /// <param name="volume">The volume to play at (0.0f to 1.0f).</param>
        /// <param name="pitch">The pitch adjustment (-1.0f to 1.0f).</param>
        /// <param name="pan">The stereo panning (-1.0f for left to 1.0f for right).</param>
        /// <param name="isLooped">Whether the sound should loop.</param>
        /// <returns>The sound effect instance that's playing.</returns>
        Microsoft.Xna.Framework.Audio.SoundEffectInstance PlaySoundEffect(string assetName, float volume, float pitch, float pan, bool isLooped);

        /// <summary>
        /// Plays a song.
        /// </summary>
        /// <param name="assetName">The asset name of the song to play.</param>
        void PlaySong(string assetName);

        /// <summary>
        /// Pauses all currently playing audio.
        /// </summary>
        void PauseAudio();

        /// <summary>
        /// Resumes all paused audio.
        /// </summary>
        void ResumeAudio();

        /// <summary>
        /// Mutes all audio.
        /// </summary>
        void MuteAudio();

        /// <summary>
        /// Unmutes all audio, restoring previous volume levels.
        /// </summary>
        void UnmuteAudio();

        /// <summary>
        /// Toggles between muted and unmuted states.
        /// </summary>
        void ToggleMute();

        /// <summary>
        /// Increases the volume of all audio by the specified amount.
        /// </summary>
        /// <param name="amount">The amount to increase (0.0f to 1.0f).</param>
        void IncreaseVolume(float amount = 0.1f);

        /// <summary>
        /// Decreases the volume of all audio by the specified amount.
        /// </summary>
        /// <param name="amount">The amount to decrease (0.0f to 1.0f).</param>
        void DecreaseVolume(float amount = 0.1f);
    }
}
