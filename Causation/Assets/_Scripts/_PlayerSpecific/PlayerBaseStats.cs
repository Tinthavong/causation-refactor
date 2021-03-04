using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseStats : MonoBehaviour, ICharacterStats
{
    //CharacterBaseStats should only handle stats, stat calculations, 
    public int maxCharacterHealth;
    [SerializeField] private int _characterHealth;
    public int CharacterHealth { get => _characterHealth; set => _characterHealth = value; }

    public int maxCharacterAmmo;
    public int shotsFired = 0; //Shots that the character has fired before reloading
    [SerializeField] private int _characterAmmo;
    public int CharacterAmmo { get => _characterAmmo; set => _characterAmmo = value; }

    [SerializeField] private int[] _characterAmmoArray = new int[3];
    public int[] CharacterAmmoArray { get => _characterAmmoArray; set => _characterAmmoArray = value; }

    //Store other ammo types here
    [SerializeField] private int _characterAmmoBlue;
    public int CharacterAmmoBlue { get => _characterAmmoBlue; set => _characterAmmoBlue = value; }

    [SerializeField] private int _characterAmmoRed;
    public int CharacterAmmoRed { get => _characterAmmoRed; set => _characterAmmoRed = value; }

    [SerializeField] private int _characterAmmoGreen;
    public int CharacterAmmoGreen { get => _characterAmmoGreen; set => _characterAmmoGreen = value; }
}
