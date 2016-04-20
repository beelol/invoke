using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Apex;
using Apex.Services;
using Apex.WorldGeometry;
using Apex.Input;
using Apex.Steering.Components;
using Apex.Steering;
using PDollarGestureRecognizer;

/// <summary>
/// Controls the player's logic. Attached to the player.
/// </summary>
public class PlayerAI : MonoBehaviour
{
    
    #region Variables

    public static List<string> learnedSpells = new List<string>();

    // Each spell and its name
    public static Dictionary<string, GameObject> spells = new Dictionary<string, GameObject>();

    public static List<Gesture> loadedGestures = new List<Gesture>();

    #region Spells

    // The basic auto-attack spell prefab.
    public GameObject Spell_BasicSpell;

    // The fireball spell prefab.
    public GameObject Spell_FireBall;

    // The wind wave spell prefab.
    public GameObject Spell_WindWave;

    // Arcane Beam spell prefab.
    public GameObject Spell_ArcaneBeam;

    // Chain Lightning spell prefab.
    public GameObject Spell_ChainLightning;

    #endregion

    #region Behavior

    bool castingSpell = false;

    // Audio source to play sounds through.
    private AudioSource _audioSource; 

    // Sound to play when the player casts a basic spell
    public AudioClip basicSpellSound;

    // Move destination of the player.
    public Vector3 dest;

    // Left hand position for spells in hand.
    public Transform SpellSpawnL;

    // Right hand position for spells in hand.
    public Transform SpellSpawnR;

    // Target to attack.
    public static Transform targetToAttack;

    // Attack range of player.
    public float attackRange = 4f;

    // Should the player follow the target?
    bool followTarget = false;

    // Cooldown before player can attack again in seconds.
    float attackCooldown = 1f;

    // The next time the player can attack.
    float nextAttackTime;

    // Used as a flag to not check to play move or stop animation.
    bool attacking = false;

    // Used as a flag to allow controlling the player.
    public bool canControlPlayer = true;

    #endregion

    #region Audio Variables

    public List<AudioClip> footstepSounds;

    public AudioSource footstepSource;

    #endregion

    #endregion

    #region Functions

    public void LearnSpell(string spell)
    {
        if(!learnedSpells.Contains(spell)) learnedSpells.Add(spell);
    }

    public static void DetectSpell(Gesture gesture, Gesture[] trainingSet)
    {
        string spellName = PointCloudRecognizer.Classify(gesture, trainingSet);

        if (spellName.Length <= 0 || !spells.ContainsKey(spellName) || !learnedSpells.Contains(spellName) ) return;

        GameObject spellToCast = spells[spellName];        

        switch (spellName)
        {
            case "Fire Ball":
                GameController.player.GetComponent<PlayerAI>().CastTargetSpell(spellToCast, 0f);
                break;
            case "Wind Wave":
                GameController.player.GetComponent<PlayerAI>().CastSelfSpell(spellToCast, 0f);
                break;
            case "Arcane Beam":
                GameController.player.GetComponent<PlayerAI>().CastTargetSpell(spellToCast, 1f);
                break;
            case "Chain Lightning":
                GameController.player.GetComponent<PlayerAI>().CastTargetSpell(spellToCast, 0f);
                break;
        }

        //print(spellName);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        dest = transform.position;
        if(!spells.ContainsKey("Fire Ball")) spells.Add("Fire Ball", Spell_FireBall);
        if (!spells.ContainsKey("Wind Wave")) spells.Add("Wind Wave", Spell_WindWave);
        if (!spells.ContainsKey("Arcane Beam")) spells.Add("Arcane Beam", Spell_ArcaneBeam);
        if (!spells.ContainsKey("Chain Lightning")) spells.Add("Chain Lightning", Spell_ChainLightning);
    }

	// Handles player controls.
	void Update () {

        if (GetComponent<Stats>().dead || !canControlPlayer)
            return;

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    CastTargetSpell(Spell_FireBall, 0f);
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    CastSelfSpell(Spell_WindWave, 0f);
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    CastTargetSpell(Spell_ArcaneBeam, 1f);
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    CastTargetSpell(Spell_ChainLightning, 0f);
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    SetDestination(Input.mousePosition, false);
        //}

        

        // If we're not following to attack, but we're far enough to move.
        if ((Vector3.Distance(transform.position, dest) > .25f && !followTarget)
            // Or if we're following to attack, but we're far enough to move.
            || (Vector3.Distance(transform.position, dest) > attackRange && followTarget))
        {
            attacking = false;

            // Play footstep sounds
            if (!footstepSource.isPlaying)
            {
                int soundToPlay = Random.Range(0, footstepSounds.Count);
                footstepSource.clip = footstepSounds[soundToPlay];
                footstepSource.Play();
            }
        }
        // If we're close enough to the destination,
        else
        {            
            // Stop moving.
            GetComponent<SteerableUnitComponent>().Stop();

            // If we're following a target to attack,
            if (followTarget)
            {
                // Look at the enemy.                
                if(targetToAttack) transform.LookAt(new Vector3(targetToAttack.position.x, transform.position.y, targetToAttack.position.z));

                if (Time.time > nextAttackTime)
                {
                    // Play the attack animation
                    if (!castingSpell)
                    {
                        Attack();
                        nextAttackTime = Time.time + attackCooldown;
                    }
                }

                //spellcasting used to be here but now the follow target check is done
                //in the method for target spells
            }
            else
            {
                attacking = false;
            }
        }

        if (!attacking)
        {
            if (GetComponent<SteerableUnitComponent>().actualVelocity.magnitude > 0.4)
            {
                GetComponent<UnitAnimator>().Move();
            }
            else if (!followTarget)
            {
                GetComponent<UnitAnimator>().Stop();
            }
        }

	}

    // Sets a destination by raycasting from the mouse.
    public void SetDestination(Vector3 destination, bool append)
    {
        if (GetComponent<Stats>().dead) return;

        RaycastHit hit;
        if (UnityServices.mainCamera.ScreenToLayerHit(destination, Layers.terrain, 1000.0f, out hit))
        {
            dest = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            var destinationBlock = hit.collider.GetComponent<InvalidDestinationComponent>();
            if (destinationBlock != null)
            {
                if (destinationBlock.entireTransform)
                {
                    return;
                }

                if (destinationBlock.onlySubArea.Contains(hit.point))
                {
                    return;
                }
            }

            //Get the transient group and send it to its destination
            //var groups = GameServices.gameStateManager.unitSelection.selected;
            //groups.MoveTo(hit.point, append);
        }

        if (UnityServices.mainCamera.ScreenToLayerHit(destination, Layers.units, 1000.0f, out hit))
        {
            // If we clicked an enemy, attack it.
            if (hit.transform.tag.Equals("Enemy"))
            {
                Transform enemy = hit.transform;

                dest = new Vector3(enemy.position.x, transform.position.y, enemy.position.z);
                // Target the enemy we clicked on.
                TargetEnemy(enemy);

                followTarget = true;
                // Have the player move to that enemy instead of to a single point.
                // We will have to change this to be constantly happening in a couroutine 
                // because enemies can move.
                GetComponent<SteerForPathComponent>().MoveTo(new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z), append);
                return;
            }
        }
        else
        {
            // If we didn't click an enemy, don't attack it.
            followTarget = false;
            DeselectTarget();
        }

        if (transform)
        {            
            GetComponent<SteerForPathComponent>().MoveTo(dest, append);
        }
    }

    /* Tells the player to attack. */
    void Attack()
    {
        
        if (targetToAttack)
        {
            GetComponent<AudioSource>().pitch = Random.Range(1f, 1.5f);
            _audioSource.PlayOneShot(basicSpellSound);
            GameObject spell = Instantiate(Spell_BasicSpell, SpellSpawnR.position, Quaternion.identity) as GameObject;
            spell.GetComponent<SpellAI>().target = targetToAttack;
            spell.GetComponent<SpellAI>().damage += GetComponent<Stats>().damage;
            attacking = true;
            GetComponent<UnitAnimator>().Attack();
        }
        else
        {
            GetComponent<UnitAnimator>().Stop();
        }
        
    }

    void CastTargetSpell(GameObject spellPrefab, float castTime)
    {
        if (!followTarget) return;


        if (targetToAttack)
        {
            castingSpell = true;
            GameObject spell = Instantiate(spellPrefab, SpellSpawnL.position, Quaternion.identity) as GameObject;

            if (castTime > 0)
            {
                spell.GetComponent<SpellAI>().lifetime = castTime;
            }

            spell.GetComponent<SpellAI>().damage += GetComponent<PlayerStats>().extraSpellDamage;
            spell.GetComponent<SpellAI>().target = targetToAttack;
            GetComponent<UnitAnimator>().Attack();
        }
        else
        {
            GetComponent<UnitAnimator>().Stop();
        }

        if (castTime > 0)
        {
            Invoke("StopCasting", castTime);
        }

        else
        {
            castingSpell = false;
        }
    }

    void CastSelfSpell(GameObject spellPrefab, float castTime) //wind radius is 1
    {
        castingSpell = true;

        GameObject spell = Instantiate(spellPrefab, transform.position, Quaternion.identity) as GameObject;
        spell.GetComponent<SelfSpellAI>().damage += GetComponent<PlayerStats>().extraSpellDamage;

        if (castTime > 0)
        {
            Invoke("StopCasting", castTime);
        }
        else
        {
            castingSpell = false;
        }
    }

    public void Die()
    {
        dest = transform.position;
    }

    private void StopCasting()
    {
        castingSpell = false;
    }

    #region TargetSelection

    private void TargetEnemy(Transform target)
    {
        // First, we untarget the targetted unit if it exists.
        DeselectTarget();

        // Then, we set the new one, and target it.
        SelectNewTarget(target);
    }

    public static void DeselectTarget()
    {
        SelectableUnit selectable;
        if (targetToAttack)
        {
            selectable = targetToAttack.GetComponent<SelectableUnit>();
            if (selectable)
            {
                selectable.SelectionVisual.SetActive(false);
            }
        }

        targetToAttack = null;
    }

    private void SelectNewTarget(Transform target)
    {
        SelectableUnit selectable;
        targetToAttack = target;

        selectable = targetToAttack.GetComponent<SelectableUnit>();
        if (selectable)
        {
            selectable.SelectionVisual.SetActive(true);
        }
    }

    #endregion

    #endregion
}
