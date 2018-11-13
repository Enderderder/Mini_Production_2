using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using XInputDotNetPure;
using InControl;

public enum ElementType
{
    Fire,
    Air,
    Earth,
    Water,

    None,
}

[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(PlayerHealthBar))]
public class Player : MonoBehaviour
{
    [Header("Config")]
    [Range(0, 1)] public int m_PlayerID;

    [Header("Stats")]
    public bool m_bIsDead = false;
    [SerializeField] private float m_MaxHealth = 100.0f;
    [SerializeField] private float m_MaxMana = 100.0f;
    [SerializeField] private float m_ManaRegenAmount = 10.0f;
    [SerializeField] private float m_ManaRegenDelay = 1.0f;

    [Header("Spells")]
    [SerializeField] private ElementType m_HoldingElement = ElementType.Earth;
    [SerializeField] private GameObject[] m_RegularSpellPrefab;
    [SerializeField] private Transform m_RegularSpellSpawnPosition;
    [SerializeField] private float m_ManaCostRegSpell = 10.0f;
    [SerializeField] private float m_ManaCostSpecialSpell = 30.0f;
    [SerializeField] private float m_SpellCastDelay = 0.5f;

    // Stats in real time
    private float m_currentHealth;
    private float m_currentMana;
    private bool m_canGenerateMana = true;

    // Components
    private InputDevice m_controller = null;
    private ThirdPersonCharacter m_character;
    private PlayerHealthBar m_healthBar;

    // Player movement
    private float m_horizontalInput;
    private float m_verticalInput;
    private Vector3 m_movementVec;
    private Transform m_playerCamera;
    private Vector3 m_camForward;

    // Player shooting
    private bool m_bCanCastSpell = true;

    // Controls
    private InputControl m_controlMoveHorizontal;
    private InputControl m_controlMoveVertical;
    private InputControl m_controlAimHorizontal;
    private InputControl m_controlAimVertical;
    private InputControl m_controlFireRegular;
    private InputControl m_controlFireSpecial;

    // Obelisk reference for mana regen
    private Obelisk m_obelisk;

    private void Awake()
    {
        m_character = GetComponent<ThirdPersonCharacter>();
        m_healthBar = GetComponent<PlayerHealthBar>();
    }

    private void Start ()
    {
        // Reference the player camera
        m_playerCamera = GameObject.Find("/PlayerCamera").transform;

        // Reference the obelisk
        m_obelisk = GameObject.FindGameObjectWithTag("Obelisk").GetComponent<Obelisk>();

        // Try assign a controller on the start
        AssignController();

        // Reset to make sure everything is fresh
        ResetStats();
    }

    private void Update ()
    {
        ProcessMovementControl();
        ProcessSpellControl();
        ProcessManaRegen();
    }

    private void FixedUpdate()
    {
        // Set value for the character scipt move
        // do not change these value
        bool jump = false;
        bool crouch = false;
        // Process the movement using the character script
        m_character.Move(m_movementVec, crouch, jump);
    }

    private void AssignController()
    {
        if (m_PlayerID >= InputManager.Devices.Count)
        {
            Debug.Log("Execeed controller total count, not enough controller connected");
            return;
        }

        m_controller = InputManager.Devices[m_PlayerID];
        StartCoroutine(ControllerVibrate(0.3f, 0.8f, 0.8f));
        Debug.Log("Player" + (m_PlayerID + 1) + " assigned device " + m_controller.Name);

        // Set up controls
        SetupControl();
    }

    private void SetupControl()
    {
        m_controlMoveHorizontal = m_controller.GetControl(InputControlType.LeftStickX);
        m_controlMoveVertical = m_controller.GetControl(InputControlType.LeftStickY);
        m_controlAimHorizontal = m_controller.GetControl(InputControlType.RightStickX);
        m_controlAimVertical = m_controller.GetControl(InputControlType.RightStickY);
        m_controlFireRegular = m_controller.GetControl(InputControlType.RightTrigger);
        m_controlFireSpecial = m_controller.GetControl(InputControlType.LeftTrigger);
    }

    private void ResetStats()
    {
        m_currentHealth = m_MaxHealth;
        m_currentMana = m_MaxMana;
        UpdateUI();
    }

    private void ProcessMovementControl()
    {
        // If there is no controller for the player
        // Try re-assign the controller
        if (m_controller == null)
        {
            AssignController();
            return;
        }

        // Get the input value
        m_horizontalInput = m_controlMoveHorizontal.Value;
        m_verticalInput = m_controlMoveVertical.Value;

        // Calculate move direction to pass to character
        if (m_playerCamera != null)
        {
            // Calculate camera relative direction to move
            m_camForward = 
                Vector3.Scale(m_playerCamera.forward, new Vector3(1, 0, 1)).normalized;
            m_movementVec = 
                m_verticalInput * m_camForward + m_horizontalInput * m_playerCamera.right;
        }
        else
        {
            // Use world-relative directions in the case of no main camera
            m_movementVec = 
                m_verticalInput * Vector3.forward + m_horizontalInput * Vector3.right;
        }
    }

    private void ProcessSpellControl()
    {
        // If there is no controller for the player
        // Try re-assign the controller
        if (m_controller == null)
        {
            AssignController();
            return;
        }

        // Shoot while able to react to user input
        // also while actually holding a element
        if (m_bCanCastSpell && m_HoldingElement != ElementType.None)
        {
            if (m_controlFireRegular.IsPressed &&
                m_currentMana >= m_ManaCostRegSpell)
            {
                StartCoroutine(CastRegularSpell());
                UseMana(m_ManaCostRegSpell);
            }
            else if (m_controlFireSpecial.IsPressed &&
                m_currentMana >= m_ManaCostSpecialSpell)
            {
                StartCoroutine(CastSpecialSpell());
                UseMana(m_ManaCostSpecialSpell);
            }
        }
    }

    private void ProcessManaRegen()
    {
        float distance = 
            Vector3.Distance(m_obelisk.transform.position, this.transform.position);

        if (distance <= m_obelisk.manaRegenRange && m_canGenerateMana)
        {
            StartCoroutine(GenerateMana());
        }

    }

    private IEnumerator CastRegularSpell()
    {
        // Spell cast action lock
        m_bCanCastSpell = false;

        GameObject spellToCast = m_RegularSpellPrefab[(int)m_HoldingElement];

        yield return new WaitForSeconds(0.1f); // Small delay waiting for the animation

        // Spawn object
        GameObject spell = Instantiate(spellToCast,
            m_RegularSpellSpawnPosition.position,
            m_RegularSpellSpawnPosition.rotation);

        // Vibrate the controller
        StartCoroutine(ControllerVibrate(0.1f, 0.1f, 1.0f));


        yield return new WaitForSeconds(m_SpellCastDelay);

        // Spell cast action unlock
        m_bCanCastSpell = true;
    }

    private IEnumerator CastSpecialSpell()
    {
        // Spell cast action lock
        m_bCanCastSpell = false;



        yield return null;


        // Spell cast action unlock
        m_bCanCastSpell = true;
    }

    private void UpdateUI()
    {
        // Update the UI health bar with percentage
        // becauze UI image fill use 0 - 1 value
        m_healthBar.ChangeHealth(m_currentHealth / m_MaxHealth);
        m_healthBar.ChangeMana(m_currentMana / m_MaxMana);
    }

    public void TakeDamage(float _damageVal)
    {
        if (m_currentHealth > 0.0f)
        {
            m_currentHealth = Mathf.Max(0.0f, m_currentHealth - _damageVal);
            StartCoroutine(DamageEffect());
            UpdateUI();
            CheckDeath();
        }
    }

    private void CheckDeath()
    {
        if (m_currentHealth <= 0.0f)
        {
            m_bIsDead = true;
            Death();
        }
    }

    private IEnumerator DamageEffect()
    {
        // Vibrate the controller
        StartCoroutine(ControllerVibrate(0.1f, 0.2f, 0.2f));

        MeshRenderer[] meshes = 
            this.gameObject.transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = new Color(1, 0, 0);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = new Color(1, 1, 1);
        }
    }

    IEnumerator GenerateMana()
    {
        m_canGenerateMana = false;
        yield return new WaitForSeconds(m_ManaRegenDelay);
        m_currentMana = Mathf.Min(m_MaxMana, m_currentMana + m_ManaRegenAmount);
        UpdateUI();
        m_canGenerateMana = true;
    }

    private void UseMana(float _value)
    {
        // Safe check
        // This will assume that player does have enough
        // to cast the spell, otherwise this will be a bug
        if (m_currentMana < _value)
        {
            Debug.Log("Insufficient amount of mana, this should not happened, its a bug");
            return;
        }

        // Use the mana and check if it is below 0
        m_currentMana = Mathf.Max(0.0f, m_currentMana - _value);
        UpdateUI();
    }

    private IEnumerator Death()
    {
        yield return null;
    }

    public void ChangeElement()
    {

    }

    private IEnumerator ControllerVibrate(float _time, float _leftMotorItens, float _rightMotorItens)
    {
        GamePad.SetVibration((PlayerIndex)m_PlayerID, _leftMotorItens, _rightMotorItens);

        yield return new WaitForSeconds(_time);

        GamePad.SetVibration((PlayerIndex)m_PlayerID, 0.0f, 0.0f);
    }
}