using System.Collections;
using UnityEngine;

//This script is control movement of left and right and attack animation to bees

public class EnemyScriptBees : MonoBehaviour
{
    [SerializeField] private float _speedOfBess; //each bee has control of speed
    [SerializeField] private float _moveDistance; // how far to move

    private Animator _animator;   

    private bool _seePlayer; // check to see if player is around
    private bool _beeHomeLocation; // this is for when bee hit (0,0,0)

    private Vector3 _positionOfBees;
    
    public GameObject _playerPostion;

    // Start is called before the first frame update
    void Start()
    {
        _positionOfBees = transform.position;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementOfBees();
    }
    
    //This is for the first animation of bees movementofbees is a function that moves left to right and homes on player for attack
    //As each prefab gets spawned it will have indavial properties reducing the pain of coding hopefully
    private void MovementOfBees()
    {
        if (!_seePlayer)
        {
            if (_beeHomeLocation == true)
            {
                _animator.SetBool("Moving_Left2Right", true);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _positionOfBees, _speedOfBess * Time.deltaTime);
                Vector3 direction = (_positionOfBees - transform.position);
                bool isTargetRight = direction.x >= 0;
                if (isTargetRight)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180f, 0);
                }
                if (transform.position == _positionOfBees)
                {
                    StartCoroutine(WaitForASecond());
                }
            }
        }
        else
        {
            _animator.enabled = false;
            transform.position = Vector2.MoveTowards(transform.position, _playerPostion.transform.position, _speedOfBess * Time.deltaTime);
            Vector3 direction = (_playerPostion.transform.position - transform.position);
            bool isTargetRight = direction.x >= 0;
            if(isTargetRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
        }
    }
    //This resets the animation to orignal state------------------------------
    IEnumerator WaitForASecond()
    {
        yield return new WaitForSeconds(0.5f);
        _beeHomeLocation = true;
        _animator.enabled = true;
        _animator.SetBool("Moving_Left2Right", false);
    }

    //Triggers ==========================================
    //Trigger check to see if player is there --------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) 
        {
            _seePlayer = true;
            _beeHomeLocation = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            _seePlayer = false;
        }
    }
}