using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class ColorGenerator 
    {
        private ColorSettings _settings;
        private Texture2D _texture;
        private const int _textureResolution = 50;

        public void UpdateSettings(ColorSettings settings)
        {
            _settings = settings;
            if(_texture == null)
                _texture = new Texture2D(_textureResolution, 1);
        }

        public void UpdateElevation(MinMax elevationMinMax)
        {
            _settings.planetMaterial.SetVector("_ElevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));
        }

        public void UpdateColors()
        {
            Color[] colors = new Color[_textureResolution];
            for (int i = 0; i < _textureResolution; i++)
                colors[i] = _settings.gradient.Evaluate(i/(_textureResolution - 1f));
            _texture.SetPixels(colors);
            _texture.Apply();
            _settings.planetMaterial.SetTexture("_PlanetTexture", _texture);
        }
    }
}
