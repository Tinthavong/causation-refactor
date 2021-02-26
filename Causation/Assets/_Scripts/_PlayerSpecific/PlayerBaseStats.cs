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
    public int shotsFired = 0; //Sloppy naming, think about something more generic
    [SerializeField]private int _characterAmmo;
    public int CharacterAmmo { get => _characterAmmo; set => _characterAmmo = value; }

    //This won't be used at all, consider getting rid of it
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
