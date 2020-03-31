using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLenght;
    public float attackDistance; //minimum distance to start attack
    public float moveSpeed;
    public float timer; //timer for cooldown between attacks

    private RaycastHit2D hit2D;
    private GameObject target;
    private Animator animator;
    private float distance; //store the distance between enemy and player
    private bool attackMode;
    private bool inRange; //check if the enemy is in range
    private bool cooling; //check if enemy is cooling after attack
    private float intTimer;

    private void Awake() {
        intTimer = timer;
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange){
            hit2D = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLenght, rayCastMask);
            RayCastDebugger();
        }

        //when player is detected by the enemy
        if(hit2D.collider != null){
            EnemyLogic();
        }
        else if(hit2D.collider == null){
            inRange = false;
        }
        
        if(inRange == false){
            animator.SetBool("nombre_de_la_animacion_del_Chino_para_caminar", false); //TODO: cambiar nombre de la animacion
            StopAttack();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            target = other.gameObject;
            inRange = true;
        }    
    }

    private void EnemyLogic(){
        distance = Vector2.Distance(transform.position, target.transform.position);

        if(distance > attackDistance){
            Move();
            StopAttack();
        }
        else if(attackDistance >= distance && cooling == false){
            Attack();
        }

        if(cooling){
            animator.SetBool("nombre_de_la_animacion_del_Chino_quieto", false); //TODO: cambiar nombre de la animacion
        }
    }

    private void RayCastDebugger(){
        if(distance > attackDistance){
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLenght, Color.red);
        }
        else if(attackDistance > distance){
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLenght, Color.green);
        }
    }

    private void Move(){
        animator.SetBool("nombre_de_la_animacion_del_Chino_para_caminar", true); //TODO: cambiar nombre de la animacion

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("nombre_de_la_animacion_del_Chino_de_ataque")){ //TODO: cambiar nombre de la animacion
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Attack(){
        timer = intTimer; //reset timer when player enter attack range
        attackMode = true; //to check if enemy can attack or not

        animator.SetBool("nombre_de_la_animacion_del_Chino_para_caminar", false); //TODO: cambiar nombre de la animacion
        animator.SetBool("nombre_de_la_animacion_del_Chino_de_ataque", true); //TODO: cambiar nombre de la animacion
    }

    private void StopAttack(){
        cooling = false;
        attackMode = false;

        animator.SetBool("nombre_de_la_animacion_del_Chino_de_ataque", false); //TODO: cambiar nombre de la animacion
    }

    public void TriggerCooling(){

    }
}
