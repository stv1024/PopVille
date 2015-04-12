using System;
using System.IO;
using Assets.Sanxiao.Communication.Proto;
using ProtoBuffer;
using UnityEngine;
using Object = System.Object;

namespace Assets.Sanxiao.Data
{
    public class ConfigManager
    {
        public enum ConfigType
        {
            CoreConfig = 0,
            RechargeConfig = 1,
            /// <summary>
            /// 3:技能配置
            /// </summary>
            SkillConfig = 2,
            SkillParameterConfig = 3,
            ExchangeConfig = 4,
            /// <summary>
            /// 6:推图关卡配置
            /// </summary>
            VegetableConfig = 5,
            ChallengeLevelConfig = 6,
            /// <summary>
            /// 8:技能等级详情文本
            /// </summary>
            SkillIntroTextConfig = 7,
            /// <summary>
            /// 9:蔬菜介绍的文本
            /// </summary>
            SkillLevelDetailTextConfig = 8,
            VegetableIntroTextConfig = 9,
            /// <summary>
            /// 10:等待的时候显示的提示文本
            /// </summary>
            WaitHintTextConfig = 10,
            CharacterConfig = 11,
            EquipConfig = 12,
            MajorLevelIntroTextConfig = 13,
            EquipIntroTextConfig = 14,
            CharacterIntroTextConfig = 15,
            OAuthParamConfig = 16,
        }

        public static readonly Object[] Configs = new object[17];

        /// <summary>
        /// 将Config载入内存
        /// </summary>
        /// <param name="configType"></param>
        /// <param name="config"></param>
        /// <returns>type正确即返回true</returns>
        public static bool SetConfig(ConfigType configType, Object config)
        {
            //Debug.LogWarning("加载" + configType + "," + config);
            var index = (int)configType;
            if (0 <= index && index < Configs.Length)
            {
                Configs[index] = config;
                return true;
            }
            return false;
        }
        public static Object GetConfig(ConfigType configType)
        {
            var index = (int) configType;
            if (0 <= index && index < Configs.Length)
            {
                return Configs[index];
            }
            return null;
        }

        //在联网前，读取Read本地缓存文件，如果没有文件，hashcode为""，第一行是hashcode
        //加载Load hashcode，数据，到内存
        //登录，比较新hashcode，相同则跳过；不同，则申请新的hashcode
        //申请的结果和运行时动态更新都是收到服务端的ResponseTextConfig，加载hashcode，数据，到内存，组装新文件替换缓存

        /// <summary>
        /// 从硬盘读取bytes加载到内存。每次刚运行时调用
        /// </summary>
        public static void ReadCacheAndLoadAllLargeConfigs()
        {
            for (int i = 0; i < Configs.Length; i++)
            {
                var configType = (ConfigType) i;

                var path = GetFilePath(configType);
                byte[] bytes = null;
                if (File.Exists(path))
                {
                    try
                    {
                        bytes = File.ReadAllBytes(path);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                if (bytes != null)
                {
                    IReceiveable configCmd = null;
                    switch (configType)
                    {
                        case ConfigType.CoreConfig:
                            configCmd = new CoreConfig();
                            break;
                        case ConfigType.RechargeConfig:
                            configCmd = new RechargeConfig();
                            break;
                        case ConfigType.SkillConfig:
                            configCmd = new SkillConfig();
                            break;
                        case ConfigType.SkillParameterConfig:
                            configCmd = new SkillParameterConfig();
                            break;
                        case ConfigType.ExchangeConfig:
                            configCmd = new ExchangeConfig();
                            break;
                        case ConfigType.VegetableConfig:
                            configCmd = new VegetableConfig();
                            break;
                        case ConfigType.ChallengeLevelConfig:
                            configCmd = new ChallengeLevelConfig();
                            break;
                        case ConfigType.SkillIntroTextConfig:
                            configCmd = new SkillIntroTextConfig();
                            break;
                        case ConfigType.SkillLevelDetailTextConfig:
                            configCmd = new SkillLevelDetailTextConfig();
                            break;
                        case ConfigType.VegetableIntroTextConfig:
                            configCmd = new VegetableIntroTextConfig();
                            break;
                        case ConfigType.WaitHintTextConfig:
                            configCmd = new WaitHintTextConfig();
                            break;
                        case ConfigType.CharacterConfig:
                            configCmd = new CharacterConfig();
                            break;
                        case ConfigType.EquipConfig:
                            configCmd = new EquipConfig();
                            break;
                        case ConfigType.MajorLevelIntroTextConfig:
                            configCmd = new MajorLevelIntroTextConfig();
                            break;
                        case ConfigType.EquipIntroTextConfig:
                            configCmd = new EquipIntroTextConfig();
                            break;
                        case ConfigType.CharacterIntroTextConfig:
                            configCmd = new CharacterIntroTextConfig();
                            break;
                        case ConfigType.OAuthParamConfig:
                            configCmd = new OAuthParamConfig();
                            break;
                        default:
                            Debug.LogError("遇到新的ConfigType,需添加代码。type:" + configType);
                            break;
                    }
                    if (configCmd != null)
                    {
                        configCmd.ParseFrom(bytes);
                        SetConfig(configType, configCmd);
                    }
                }
            }
        }

        /// <summary>
        /// 从服务端获得新的配置时，调用
        /// </summary>
        public static void Execute(Config cmd)
        {
            ConfigType configType;
            Object o;

            #region CoreConfig
            if (cmd.HasCoreConfig && cmd.CoreConfig != null)
            {
                configType = ConfigType.CoreConfig;
                var newConfig = cmd.CoreConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as CoreConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region RechargeConfig
            if (cmd.HasRechargeConfig && cmd.RechargeConfig != null)
            {
                configType = ConfigType.RechargeConfig;
                var newConfig = cmd.RechargeConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as RechargeConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region SkillConfig
            if (cmd.HasSkillConfig && cmd.SkillConfig != null)
            {
                configType = ConfigType.SkillConfig;
                var newConfig = cmd.SkillConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as SkillConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region SkillParameterConfig
            if (cmd.HasSkillParameterConfig && cmd.SkillParameterConfig != null)
            {
                configType = ConfigType.SkillParameterConfig;
                var newConfig = cmd.SkillParameterConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as SkillParameterConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region ExchangeConfig
            if (cmd.HasExchangeConfig && cmd.ExchangeConfig != null)
            {
                configType = ConfigType.ExchangeConfig;
                var newConfig = cmd.ExchangeConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as ExchangeConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region VegetableConfig
            if (cmd.HasVegetableConfig && cmd.VegetableConfig != null)
            {
                configType = ConfigType.VegetableConfig;
                var newConfig = cmd.VegetableConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as VegetableConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region ChallengeLevelConfig
            if (cmd.HasChallengeLevelConfig && cmd.ChallengeLevelConfig != null)
            {
                configType = ConfigType.ChallengeLevelConfig;
                var newConfig = cmd.ChallengeLevelConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as ChallengeLevelConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region SkillIntroTextConfig
            if (cmd.HasSkillIntroTextConfig && cmd.SkillIntroTextConfig != null)
            {
                configType = ConfigType.SkillIntroTextConfig;
                var newConfig = cmd.SkillIntroTextConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as SkillIntroTextConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region SkillLevelDetailTextConfig
            if (cmd.HasSkillLevelDetailTextConfig && cmd.SkillLevelDetailTextConfig != null)
            {
                configType = ConfigType.SkillLevelDetailTextConfig;
                var newConfig = cmd.SkillLevelDetailTextConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as SkillLevelDetailTextConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region VegetableIntroTextConfig
            if (cmd.HasVegetableIntroTextConfig && cmd.VegetableIntroTextConfig != null)
            {
                configType = ConfigType.VegetableIntroTextConfig;
                var newConfig = cmd.VegetableIntroTextConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as VegetableIntroTextConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region WaitHintTextConfig
            if (cmd.HasWaitHintTextConfig && cmd.WaitHintTextConfig != null)
            {
                configType = ConfigType.WaitHintTextConfig;
                var newConfig = cmd.WaitHintTextConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as WaitHintTextConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region CharacterConfig
            if (cmd.HasCharacterConfig && cmd.CharacterConfig != null)
            {
                configType = ConfigType.CharacterConfig;
                var newConfig = cmd.CharacterConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as CharacterConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region EquipConfig
            if (cmd.HasEquipConfig && cmd.EquipConfig != null)
            {
                configType = ConfigType.EquipConfig;
                var newConfig = cmd.EquipConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as EquipConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region MajorLevelIntroTextConfig
            if (cmd.HasMajorLevelIntroTextConfig && cmd.MajorLevelIntroTextConfig != null)
            {
                configType = ConfigType.MajorLevelIntroTextConfig;
                var newConfig = cmd.MajorLevelIntroTextConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as MajorLevelIntroTextConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region EquipIntroTextConfig
            if (cmd.HasEquipIntroTextConfig && cmd.EquipIntroTextConfig != null)
            {
                configType = ConfigType.EquipIntroTextConfig;
                var newConfig = cmd.EquipIntroTextConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as EquipIntroTextConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region CharacterIntroTextConfig
            if (cmd.HasCharacterIntroTextConfig && cmd.CharacterIntroTextConfig != null)
            {
                configType = ConfigType.CharacterIntroTextConfig;
                var newConfig = cmd.CharacterIntroTextConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as CharacterIntroTextConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion

            #region OAuthParamConfig
            if (cmd.HasOauthParamConfig && cmd.OauthParamConfig != null)
            {
                configType = ConfigType.OAuthParamConfig;
                var newConfig = cmd.OauthParamConfig;
                o = GetConfig(configType);
                var oldConfig = o != null ? (o as OAuthParamConfig) : null;
                if (oldConfig == null || oldConfig.Hash != newConfig.Hash)
                {
                    if (SetConfig(configType, newConfig))
                    {
                        ForceCache(configType, newConfig.GetProtoBufferBytes());
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 将大配置写入文件
        /// </summary>
        /// <param name="configType"></param>
        /// <param name="bytes"></param>
        public static void ForceCache(ConfigType configType, byte[] bytes)
        {
            if (bytes == null) return;
            var path = GetFilePath(configType);
            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// 将大配置写入文件
        /// </summary>
        public static void DeleteAll()
        {
            for (var i = 0; i < Configs.Length; i++)
            {
                var configType = (ConfigType) i;
                var path = GetFilePath(configType);
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        #region Util
        public static string GetFilePath(ConfigType configType)
        {
            return Path.Combine(Application.persistentDataPath, configType + ".config");
        }

        public static string GetHash(ConfigType configType)
        {
            var config = GetConfig(configType);
            switch (configType)
            {
                case ConfigType.CoreConfig:
                    {
                        var c = config as CoreConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.RechargeConfig:
                    {
                        var c = config as RechargeConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.SkillConfig:
                    {
                        var c = config as SkillConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.SkillParameterConfig:
                    {
                        var c = config as SkillParameterConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.ExchangeConfig:
                    {
                        var c = config as ExchangeConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.VegetableConfig:
                    {
                        var c = config as VegetableConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.ChallengeLevelConfig:
                    {
                        var c = config as ChallengeLevelConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.SkillIntroTextConfig:
                    {
                        var c = config as SkillIntroTextConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.SkillLevelDetailTextConfig:
                    {
                        var c = config as SkillLevelDetailTextConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.VegetableIntroTextConfig:
                    {
                        var c = config as VegetableIntroTextConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.WaitHintTextConfig:
                    {
                        var c = config as WaitHintTextConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.CharacterConfig:
                    {
                        var c = config as CharacterConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.EquipConfig:
                    {
                        var c = config as EquipConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.MajorLevelIntroTextConfig:
                    {
                        var c = config as MajorLevelIntroTextConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.EquipIntroTextConfig:
                    {
                        var c = config as EquipIntroTextConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.CharacterIntroTextConfig:
                    {
                        var c = config as CharacterIntroTextConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                case ConfigType.OAuthParamConfig:
                    {
                        var c = config as OAuthParamConfig;
                        if (c != null) return c.Hash;
                    }
                    break;
                default:
                    Debug.LogException(new ArgumentOutOfRangeException("configType:" + configType));
                    return null;
            }
            return null;
        }
        //public static Type GetTypeFromConfigType(ConfigType configType)
        //{
        //    return typeof (RechargeConfig);
        //}
        #endregion
    }
}
