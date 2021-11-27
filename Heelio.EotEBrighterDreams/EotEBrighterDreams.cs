using OWML.ModHelper;
using OWML.Common;
using UnityEngine;

namespace EotEBrighterDreams
{
    public class EotEBrighterDreams : ModBehaviour
    {
        DreamLanternController _dreamLanternController;
        Light _light;
        bool _isLoaded;

        float _lightRange = 10f;
        float _lightIntensity = .1f;
        bool _enabled = true;

        void Start()
        {
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                _isLoaded = false;
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

                ModHelper.Events.Subscribe<DreamLanternController>(Events.AfterStart);
                ModHelper.Events.Event += OnEvent;
            };
        }

        void Update()
        {
            if (!_isLoaded || !_enabled)
                return;

            _light.gameObject.SetActive(_dreamLanternController.isActiveAndEnabled);
        }
        
        void OnEvent(MonoBehaviour behaviour, Events ev)
        {
            if (behaviour is DreamLanternController dlc && ev == Events.AfterStart)
            {
                _dreamLanternController = dlc;
                _isLoaded = true;
            }
        }

        void UpdateDreamDimLightOptions()
        {
            _light.range = _lightRange;
            _light.intensity = _lightIntensity;

            if (!_enabled)
                _light.gameObject.SetActive(false);
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
