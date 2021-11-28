using OWML.ModHelper;
using OWML.Common;
using UnityEngine;

namespace EotEBrighterDreams
{
    public class EotEBrighterDreams : ModBehaviour
    {
        Light _light;
        bool _isInDreamWorld;

        float _lightRange = 10f;
        float _lightIntensity = .1f;
        bool _enabled = true;

        void Start()
        {
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                if (loadScene != OWScene.SolarSystem) return;

                PlayerBody playerBody = FindObjectOfType<PlayerBody>();

                GameObject lightGO = new GameObject("DreamDimLight");
                lightGO.transform.parent = playerBody.transform;
                lightGO.transform.localPosition = Vector3.zero;

                _light = lightGO.AddComponent<Light>();
                _light.type = LightType.Point;
                _light.color = Color.white;
                _light.shadows = LightShadows.None;

                UpdateDreamDimLightOptions();

                GlobalMessenger.AddListener("EnterDreamWorld", OnEnterDreamWorld);
                GlobalMessenger.AddListener("ExitDreamWorld", OnExitDreamWorld);
            };
        }

        void OnEnterDreamWorld()
        {
            _isInDreamWorld = true;
            _light.gameObject.SetActive(_enabled);
        }

        void OnExitDreamWorld()
        {
            _isInDreamWorld = false;
            _light.gameObject.SetActive(false);
        }

        void UpdateDreamDimLightOptions()
        {
            _light.range = _lightRange;
            _light.intensity = _lightIntensity;

            if (_isInDreamWorld)
                _light.gameObject.SetActive(_enabled);
        }

        public override void Configure(IModConfig config)
        {
            _lightRange = config.GetSettingsValue<float>("range");
            _lightIntensity = config.GetSettingsValue<float>("intensity") / 10f;
            _enabled = config.Enabled;

            if (_light != null)
                UpdateDreamDimLightOptions();
        }
    }
}
