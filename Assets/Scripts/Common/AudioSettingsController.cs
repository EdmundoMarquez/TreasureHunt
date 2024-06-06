using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;

namespace Treasure.Audio
{
    public class AudioSettingsController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer = null;
        [SerializeField] private string _volumeParameter = null;
        [SerializeField] private Slider _slider = null;
        [SerializeField] private float _multiplier = 30f;
        [SerializeField] private Button _toggleButton = null;
        [SerializeField] private Image _toggleIcon = null;
        // [SerializeField] private GameSaveData _playerData = null;
        private bool toggleSlider = true;
        private void Awake()
        {
            if (_slider == null) return;
            _slider.onValueChanged.AddListener(HandleSliderValueChanged);
            _toggleButton.onClick.AddListener(ToggleSlider);
        }
        private void Start()
        {
            // if (_slider != null)
            //     _slider.value = _playerData.PlayerPreferences.MusicVolumen;
            // HandleSliderValueChanged(_playerData.PlayerPreferences.MusicVolumen);
        }
        private void HandleSliderValueChanged(float value)
        {
            // _playerData.PlayerPreferences.MusicVolumen = value;
            _audioMixer.SetFloat(_volumeParameter, Mathf.Log10(value) * _multiplier);
        }

        private void ToggleSlider()
        {
            toggleSlider = !toggleSlider;
            _toggleIcon.DOFade(toggleSlider ? 1f : 0.5f, 0.3f).SetUpdate(true);
            _slider.interactable = toggleSlider;

            if(toggleSlider)
                _audioMixer.SetFloat(_volumeParameter, Mathf.Log10(_slider.value) * _multiplier);
            else
                _audioMixer.SetFloat(_volumeParameter, Mathf.Log10(0f) * _multiplier);

        }
    }
}