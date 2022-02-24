using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using IPA.Utilities;
using ReProcessor.Configuration;
using UnityEngine;
using ReProcessor.Files;
using Camera = UnityEngine.Camera;
namespace ReProcessor.Extensions
{
    

    static class Reflection
    {

        internal static FieldInfo _PrivateField(this object input, string fieldName)
        {
            return null;
            //return input.GetPrivateField<FieldInfo>(fieldName);
            Type type = input.GetType();
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            return type.GetField(fieldName, bindingFlags);
        }
        
        internal static void SetPrivateField(this object input, string fieldName, object value)
        {
            input.SetField(fieldName,value);
        }
        internal static MainEffectController MainEffectController(this Camera cam)
        {
            return cam.gameObject.GetComponent(typeof(MainEffectController)) as MainEffectController;
        }
        internal static MainEffectContainerSO MainEffectContainerSO(this Camera cam)
        {
            MainEffectController ctl = cam.gameObject.GetComponent<MainEffectController>();
            return ctl.GetField<MainEffectContainerSO, MainEffectController>("_mainEffectContainer");
            return ctl.GetComponent<MainEffectContainerSO>();
            return ctl.GetProperty<MainEffectContainerSO, MainEffectController>("_mainEffectContainer");
            
        }
        internal static void SetCameraSetting(this Camera cam, string fieldName, object value)
        {
            //Plugin.Log.Notice($"Setting camera value as type \"{value.GetType()}\"");
            cam.MainEffectContainerSO().mainEffect.SetPrivateField(fieldName, value);
        }

        internal static bool BloomOn()
        {
            var t = Camera.main.MainEffectContainerSO().mainEffect.GetType();
            return t == typeof(PyramidBloomMainEffectSO);
        }
        internal static void SetCameraSetting(this Camera cam, CameraSetting camSetting)
        {
            var ef = cam.MainEffectContainerSO();
            
            var eff = ef.GetField<MainEffectSO, MainEffectContainerSO>("_mainEffect");
            if(eff is PyramidBloomMainEffectSO pyramid)
                pyramid.SetProperty(camSetting.PropertyName,camSetting.Value);
            
            //Plugin.Log.Notice($"Camera value is of type \"{camSetting.Value.GetType()}\"");
            /*
            if (camSetting.ValueType.Equals(typeof(PyramidBloomRendererSO.Pass)))
                f.SetValue(eff,camSetting.Value.ToPass());
            else if (camSetting.ValueType.EqualsInt())
                f.SetValue(eff,Convert.ToInt32(camSetting.Value));
            else if (camSetting.ValueType.Equals(typeof(Single)))
                f.SetValue(eff,Convert.ToSingle(camSetting.Value));
            else
                f.SetValue(eff, camSetting.Value);*/
        }
        internal static void ApplySettings(this Camera cam, List<CameraSetting> camSettings)
        {
            foreach(var setting in camSettings)
            {
                //Console.WriteLine($"ReProcessor.Extensions.ApplySettings | setting \"{setting.FriendlyName}\" has a value of \"{setting.Value.ToString()}\" and a type of \"{setting.Value.GetType().ToString()}\"");

                

                if (setting.ValueType.EqualsInt())
                    cam.SetCameraSetting(setting.PropertyName, Int32.Parse(setting.Value.ToString()));
                else if (setting.ValueType == typeof(System.Single))
                    cam.SetCameraSetting(setting.PropertyName, float.Parse(setting.Value.ToString()));
                
                else if (setting.ValueType == typeof(PyramidBloomRendererSO.Pass))
                    cam.SetCameraSetting(setting.PropertyName, setting.Value);
            }
        }
        internal static CameraSetting GetSetting(this List<CameraSetting> list, string property)
        {
            CameraSetting retrn = null!;
            foreach(var setting in list)
            {
                if (setting.PropertyName == property)
                    retrn = setting;
                else
                    retrn = null;
            }
            return retrn;
        }
        
        internal static bool EqualsInt(this Type type)
        {
            return (type.Equals(typeof(System.Int16)) || type.Equals(typeof(System.Int32)) || type.Equals(typeof(System.Int64)));
        }


        public static PyramidBloomRendererSO.Pass ToPass(this object pass)
        {
            switch (pass)
            {
                case "Prefilter13":
                    return PyramidBloomRendererSO.Pass.Prefilter13;
                case "Prefilter4":
                    return PyramidBloomRendererSO.Pass.Prefilter4;
                case "Downsample13":
                    return PyramidBloomRendererSO.Pass.Downsample13;
                case "Downsample4":
                    return PyramidBloomRendererSO.Pass.Downsample4;
                case "DownsampleBilinearGamma":
                    return PyramidBloomRendererSO.Pass.DownsampleBilinearGamma;
                case "UpsampleTent":
                    return PyramidBloomRendererSO.Pass.UpsampleTent;
                case "UpsampleBox":
                    return PyramidBloomRendererSO.Pass.UpsampleBox;
                case "UpsampleTentGamma":
                    return PyramidBloomRendererSO.Pass.UpsampleTentGamma;
                case "UpsampleBoxGamma":
                    return PyramidBloomRendererSO.Pass.UpsampleBoxGamma;
                case "Bilinear":
                    return PyramidBloomRendererSO.Pass.Bilinear;
                case "BilinearGamma":
                    return PyramidBloomRendererSO.Pass.BilinearGamma;
                case "UpsampleTentAndReinhardToneMapping":
                    return PyramidBloomRendererSO.Pass.UpsampleTentAndReinhardToneMapping;
                case "UpsampleTentAndACESToneMapping":
                    return PyramidBloomRendererSO.Pass.UpsampleTentAndACESToneMapping;
                default:
                    return PyramidBloomRendererSO.Pass.Bilinear;
            }
        }
        public static string ToString(this PyramidBloomRendererSO.Pass pass)
        {
            switch (pass)
            {
                case PyramidBloomRendererSO.Pass.Prefilter13:
                    return "Prefilter13";
                case PyramidBloomRendererSO.Pass.Prefilter4:
                    return "Prefilter4";
                case PyramidBloomRendererSO.Pass.Downsample13:
                    return "Downsample13";
                case PyramidBloomRendererSO.Pass.Downsample4:
                    return "Downsample4";
                case PyramidBloomRendererSO.Pass.DownsampleBilinearGamma:
                    return "DownsampleBilinearGamma";
                case PyramidBloomRendererSO.Pass.UpsampleTent:
                    return "UpsampleTent";
                case PyramidBloomRendererSO.Pass.UpsampleBox:
                    return "UpsampleBox";
                case PyramidBloomRendererSO.Pass.UpsampleTentGamma:
                    return "UpsampleTentGamma";
                case PyramidBloomRendererSO.Pass.UpsampleBoxGamma:
                    return "UpsampleBoxGamma";
                case PyramidBloomRendererSO.Pass.Bilinear:
                    return "Bilinear";
                case PyramidBloomRendererSO.Pass.BilinearGamma:
                    return "BilinearGamma";
                case PyramidBloomRendererSO.Pass.UpsampleTentAndReinhardToneMapping:
                    return "UpsampleTentAndReinhardToneMapping";
                case PyramidBloomRendererSO.Pass.UpsampleTentAndACESToneMapping:
                    return "UpsampleTentAndACESToneMapping";
                default:
                    return "null";
            }
        }

    }
}
