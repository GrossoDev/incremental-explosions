using UnityEngine;

namespace Battle.View
{
    [RequireComponent(typeof(Light))]
    public class TorchLightView : MonoBehaviour
    {
        [Header("Torch Light")]
        [SerializeField] private float baseIntensity = 4.5f;
        [SerializeField] private float intensityAmplitude = 1.1f;
        [SerializeField] private float baseRange = 5.5f;
        [SerializeField] private float rangeAmplitude = 0.55f;

        [Header("Fire Flicker")]
        [SerializeField] private float flickerSpeed = 6.5f;
        [SerializeField] private float positionSwayAmplitude = 0.08f;
        [SerializeField] private Color darkFlameColor = new Color(1f, 0.45f, 0.12f);
        [SerializeField] private Color brightFlameColor = new Color(1f, 0.78f, 0.35f);

        private Light torchLight;
        private Vector3 baseLocalPosition;
        private float noiseOffset;

        private void Awake()
        {
            torchLight = GetComponent<Light>();
            torchLight.renderMode = LightRenderMode.ForcePixel;
            baseLocalPosition = transform.localPosition;
            noiseOffset = Random.Range(0f, 100f);
        }

        private void Update()
        {
            float time = Time.time * flickerSpeed;
            float primary = Mathf.PerlinNoise(noiseOffset, time);
            float secondary = Mathf.PerlinNoise(noiseOffset + 37.3f, time * 2.35f);
            float combined = primary * 0.65f + secondary * 0.35f;
            float centered = (combined - 0.5f) * 2f;

            torchLight.intensity = Mathf.Max(0f, baseIntensity + centered * intensityAmplitude);
            torchLight.range = Mathf.Max(0.1f, baseRange + centered * rangeAmplitude);
            torchLight.color = Color.Lerp(darkFlameColor, brightFlameColor, combined);

            if (positionSwayAmplitude > 0f)
            {
                float swayX = (Mathf.PerlinNoise(noiseOffset + 11.1f, time * 1.6f) - 0.5f) * 2f * positionSwayAmplitude;
                float swayZ = (Mathf.PerlinNoise(noiseOffset + 73.7f, time * 1.6f) - 0.5f) * 2f * positionSwayAmplitude;
                transform.localPosition = baseLocalPosition + new Vector3(swayX, 0f, swayZ);
            }
        }

        private void OnValidate()
        {
            baseIntensity = Mathf.Max(0f, baseIntensity);
            baseRange = Mathf.Max(0.1f, baseRange);
            intensityAmplitude = Mathf.Max(0f, intensityAmplitude);
            rangeAmplitude = Mathf.Max(0f, rangeAmplitude);
            flickerSpeed = Mathf.Max(0f, flickerSpeed);
            positionSwayAmplitude = Mathf.Max(0f, positionSwayAmplitude);

            if (TryGetComponent(out Light lightComponent))
            {
                lightComponent.intensity = baseIntensity;
                lightComponent.range = baseRange;
            }
        }
    }
}

