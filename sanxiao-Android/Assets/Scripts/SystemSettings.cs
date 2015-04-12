using UnityEngine;

namespace Assets.Scripts
{
    public class SystemSettings
    {
        public static bool MusicOn;
        public static bool AudioOn;

        public static void SaveSettingsToHD()
        {
            PlayerPrefs.SetFloat("MusicOn", MusicOn ? 1 : 0);
            PlayerPrefs.SetFloat("AudioOn", AudioOn ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void LoadSettingsFromHD()
        {
            MusicOn = PlayerPrefs.GetInt("MusicOn", 1) > 0;
            AudioOn = PlayerPrefs.GetInt("AudioOn", 1) > 0;
        }

        public static void ClearSettingsOnHD()
        {
            PlayerPrefs.DeleteKey("MusicOn");
            PlayerPrefs.DeleteKey("AudioOn");
            PlayerPrefs.DeleteAll();
        }
    }
}