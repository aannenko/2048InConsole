using System.Configuration;

namespace _2048InConsole
{
    internal class GameSettings
    {
        internal readonly byte Columns;
        internal readonly byte Rows;
        internal readonly byte ColumnSize;
        internal readonly bool DynamicElapsedTime;

        private const byte DefaultColumnsOrRows = 4;
        private const byte MinColumnsOrRows = 2;
        private const byte DefaultColumnSize = 8;
        private const byte MinColumnSize = 6;
        private const bool DefaultDynamicElapsedTime = false;

        internal GameSettings()
        {
            Columns = GetByteSetting(nameof(Columns), DefaultColumnsOrRows, MinColumnsOrRows);
            Rows = GetByteSetting(nameof(Rows), DefaultColumnsOrRows, MinColumnsOrRows);
            ColumnSize = GetByteSetting(nameof(ColumnSize), DefaultColumnSize, MinColumnSize);
            DynamicElapsedTime = GetBoolSetting(nameof(DynamicElapsedTime), DefaultDynamicElapsedTime);
        }

        private static byte GetByteSetting(string settingName, byte defaultValue, byte minValue)
        {
            byte setting;
            return byte.TryParse(ConfigurationManager.AppSettings[settingName], out setting) && setting > minValue
                ? setting
                : defaultValue;
        }

        private static bool GetBoolSetting(string settingName, bool defaultValue)
        {
            bool setting;
            return bool.TryParse(ConfigurationManager.AppSettings[settingName], out setting)
                ? setting
                : defaultValue;
        }
    }
}
