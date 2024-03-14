using UnityEngine;
using Cinemachine;

public class CharacterSwitcher : MonoBehaviour
{
    
    [SerializeField] private CinemachineVirtualCamera _followCamera = null;
    [SerializeField] private Warrior _warriorCharacter = null;
    [SerializeField] private Sage _sageCharacter = null;
    private bool isWarriorActive = true;

    private void Awake()
    {
        //Set warrior as the default
        _warriorCharacter.ToggleControl(true);
        _sageCharacter.ToggleControl(false);
        isWarriorActive = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            isWarriorActive = !isWarriorActive;
            _warriorCharacter.ToggleControl(isWarriorActive);
            _sageCharacter.ToggleControl(!isWarriorActive);

            _followCamera.m_Follow = isWarriorActive ? _warriorCharacter.transform : _sageCharacter.transform;
        }
    }

}
