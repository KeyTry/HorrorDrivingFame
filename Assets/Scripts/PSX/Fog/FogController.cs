using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PSX
{
    [ExecuteInEditMode]
    public class FogController : MonoBehaviour
    {
        [SerializeField] protected VolumeProfile volumeProfile;
        [SerializeField] protected bool isEnabled = true;

        protected Fog fog;

        [Range(0, 10)]
        [SerializeField] private float fogDensity = 1.0f;
        [Range(0, 20)]
        [SerializeField] private float fogDistance = 10.0f;
        [Range(0, 100)]
        [SerializeField] private float fogNear = 1.0f;
        [Range(0, 100)]
        [SerializeField] private float fogFar = 100.0f;
        [Range(0, 100)]
        [SerializeField] private float fogAltScale = 10.0f;
        [Range(0, 1000)]
        [SerializeField] private float fogThinning = 100.0f;
        [Range(0, 1000)]
        [SerializeField] private float noiseScale = 100.0f;
        [Range(0, 1)]
        [SerializeField] private float noiseStrength = 0.05f;

        [SerializeField] private Color fogColor;

        public float FogDensity { get => fogDensity; set => fogDensity = value; }
        public float FogDistance { get => fogDistance; set => fogDistance = value; }
        public float FogNear { get => fogNear; set => fogNear = value; }
        public float FogFar { get => fogFar; set => fogFar = value; }
        public float FogAltScale { get => fogAltScale; set => fogAltScale = value; }
        public float FogThinning { get => fogThinning; set => fogThinning = value; }
        public float NoiseScale { get => noiseScale; set => noiseScale = value; }
        public float NoiseStrength { get => noiseStrength; set => noiseStrength = value; }
        public Color FogColor { get => fogColor; set => fogColor = value; }

        protected void Update()
        {
            this.SetParams();
        }

        protected void SetParams()
        {
            if (!this.isEnabled) return; 
            if (this.volumeProfile == null) return;
            if (this.fog == null) volumeProfile.TryGet<Fog>(out this.fog);
            if (this.fog == null) return;
            
            
            this.fog.fogDensity.value = this.fogDensity;
            this.fog.fogDistance.value = this.fogDistance;
            this.fog.fogNear.value = this.fogNear;
            this.fog.fogFar.value = this.fogFar;
            this.fog.fogAltScale.value = this.fogAltScale;
            this.fog.fogThinning.value = this.fogThinning;
            this.fog.noiseScale.value = this.noiseScale;
            this.fog.noiseStrength.value = this.noiseStrength;
            this.fog.fogColor.value = this.fogColor;
            
            
            //ACCESSING PARAMS 
            // this.fog.parameters.value
        }
    }
}