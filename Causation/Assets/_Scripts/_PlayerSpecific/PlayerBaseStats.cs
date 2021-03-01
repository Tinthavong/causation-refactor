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
    [SerializeField]private int _characterAmmo;
    public int CharacterAmmo { get => _characterAmmo; set => _characterAmmo = value; }
}
