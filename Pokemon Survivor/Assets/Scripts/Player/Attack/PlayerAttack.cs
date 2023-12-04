using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerStats playerStats;
    DialogueManager dialogueManager;
    Camera cam;
    #pragma warning disable CS8632
    public Transform? proyectileSpawnPoint;

    #pragma warning disable CS8632
    public GameObject? firstAttack;

    #pragma warning disable CS8632
    public GameObject? altAttack;

    #pragma warning disable CS8632
    public GameObject? secondaryAttack;
    public GameObject secAttackEffect;

    #pragma warning restore CS8632
    public Animator animator;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    // it's a multiplier, every attack has his damage points but always multiplied by this
    // multiplier which value has to be 1, in the moment that the player takes a powerup
    // this multiplier is going to increase.

    public float damage = 1f; 

    public float meleeRange;
    float maxMeleeCap = 2f;
    float minMeleeCap = 1f;

    
    public float initialCooldown = 1.5f;
    public float attackCooldown;
    int altAttackCount = 0;

    float nextAttack;
    public Vector2 mousePosition;

    bool isAttackDeployed = false;
    private bool choiceBandApply = false;
    private bool muscleBraceApply = false;
    private bool wiseGlassesApply = false;

    /*public static bool crit = false;
    public static float critDamage;

*/
    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }
    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        attackCooldown = initialCooldown;
        nextAttack = 0;
    }
    void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        nextAttack -= Time.deltaTime;
        if (!pauseMenu.activeSelf && !gameOverMenu.activeSelf && Input.GetButtonDown("Fire1") && !KecleonShop.shoping)
            FirstAttack();

        // applying the shop item effect: CHOICE BAND.

        ShopItem choiceBand = FindObjectOfType<GameController>().shopItems.Find(item => item.name.Equals("Cinta Eleccion"));

        if(choiceBand.active && !dialogueManager.inConversation) 
        {
            if(!choiceBandApply)
            {

                damage += damage * (choiceBand.value / 100); // this values are %.

                choiceBandApply = true;
            }
        }

        // applying the shop item effect: MUSCLE BRACE, power ups the melee attack

        ShopItem muscleBrace = FindObjectOfType<GameController>().shopItems.Find(item => item.name.Equals("Cinta Fuerte"));

        if(muscleBrace.active && !muscleBraceApply && firstAttack.CompareTag("Melee")) // we only apply that items on player which attacks are melee
        {
            damage += damage * (muscleBrace.value / 100);

            muscleBraceApply = true;
        }

        // applying the shop item effect: WISE GLASSES, increase proyectile damage

        ShopItem wiseGlasses = FindObjectOfType<GameController>().shopItems.Find(item => item.name.Equals("Gafas Especiales"));

        if(wiseGlasses.active && !wiseGlassesApply && firstAttack.CompareTag("Proyectile"))
        {
            damage += damage * (wiseGlasses.value / 100);
        
            wiseGlassesApply = true;
        }

        if (!pauseMenu.activeSelf && !gameOverMenu.activeSelf && Input.GetButtonDown("Fire2") && !KecleonShop.shoping && !choiceBand.active)
            SecondaryAttack();

        if (isAttackDeployed) // Little animation on strong attack bar.
        {
            playerStats.strongAttackPoints -= 5f;
            if (playerStats.strongAttackPoints <= 0)
            {
                playerStats.strongAttackPoints = 0;
                isAttackDeployed = false;
            }
        }

        if (meleeRange >= maxMeleeCap) // Cap the max melee range at 2
            meleeRange = maxMeleeCap;

        if(meleeRange <= minMeleeCap) // Cap the min melee range at 1
            meleeRange = minMeleeCap;
    }

    void FirstAttack()
    {
        if (firstAttack)
        {
            if (nextAttack <= 0)
            {
                animator.SetBool("Attack", true);

                

                switch (firstAttack.tag)
                {
                    case "Proyectile":

                        if (altAttack != null && altAttackCount == 2)
                        {
                            Instantiate(altAttack, proyectileSpawnPoint.position, proyectileSpawnPoint.rotation);
                            altAttackCount = 0;
                        }
                        else
                        {
                            Instantiate(firstAttack, proyectileSpawnPoint.position, proyectileSpawnPoint.rotation);
                            altAttackCount++;
                        }

                    break;

                    case "Melee":
                        
                        // Sacamos el area donde se puede golpear al enemigo
                        // esto depende del rango de ataque
                        Vector2 spawnAttackArea = new Vector3(this.transform.position.x + meleeRange, this.transform.position.y + meleeRange);

                        // Calculamos las distancias, primero entre el personaje y el rango m�ximo de ataque
                        // y luego entre el jugador y d�nde se hace click (el spawn del ataque)
                        float range = Vector3.Distance(this.transform.position, spawnAttackArea);

                        float spawn = Vector3.Distance(this.transform.position, mousePosition);

                        // El problema es que el rango de ataque establecido y el valor del rango que se calcula son diferentes
                        // y hay que calcular la diferencia entre ambos:
                        float diference = range - meleeRange;

                        // El rango real, donde se puede hacer spawn del ataque, es la resta
                        // del rango calculado por la diferencia de rango ===> x = 2.1 - 0.6 por ejemplo; x = 1.5 (exactamente el mismo valor que hayamos colocado al rango de ataque)
                        float realRange = range - diference;

                        // si la posicion del spawn es menor a la posicion del rango 
                        // es decir si el spawn est� dentro de rango
                        if (spawn <= realRange)
                            // Spawneamos el ataque
                            Instantiate(firstAttack, mousePosition, Quaternion.identity);
                        else
                            // Sino, restablecemos el animador del personaje para que no quede pegando infinitamente.
                            animator.SetBool("Attack", false);
                    break;

                    default:

                        playerStats = this.gameObject.GetComponent<PlayerStats>();

                        if (playerStats.gameObject.name.Contains("Pikachu") || playerStats.gameObject.name.Contains("Raichu"))
                        {
                            Vector2 rangedPosition = new Vector2(mousePosition.x, (mousePosition.y += 2f));
                        }

                        Instantiate(firstAttack, mousePosition, Quaternion.identity);

                    break;
                }

                nextAttack = attackCooldown;
            }
        }
    }

    void SecondaryAttack()
    {
        playerStats = this.gameObject.GetComponent<PlayerStats>();
        if (playerStats.strongAttackPoints >= playerStats.strongAttackMaxPoints)
        {
            GoodStatusCondition.applyEffect = true;
            
            if (secondaryAttack != null)
            {
                switch(secondaryAttack.tag)
                {
                    case "SecDamage": Instantiate(secondaryAttack, mousePosition, Quaternion.identity); break;

                    case "SecStatus":
                        secAttackEffect.GetComponent<Animator>().Rebind();
                        secAttackEffect.SetActive(true);
                    break;
                }
            }
            animator.SetBool("SecAttack", true);
            isAttackDeployed = true;
        }
    }

    public void PlayAnimSound(string name)
    {
        FindObjectOfType<AudioManager>().PlaySound(name);

        // if the attack its recovery from mew: we start to heal with recovery ability
        // we allow with this play both animations at same time

        if(name.Equals("Recovery"))
        {
            Recovery.recovering = true;

            Recovery.showAnimation = true;
        }
    }

    public void EndStatusConditionAttack()
    {
        animator.SetBool("SecAttack", false);
        secAttackEffect.SetActive(false);
    }

    void EndAnimation()
    {
        animator.SetBool("SecAttack", false);
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, secAttackRange);*/

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, meleeRange);

    }

}
