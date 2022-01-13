using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("StartingPatrol")]
    //On déclare nos variables
    [SerializeField] float _speed = 2f;
    [SerializeField, Range(0.1f, 50f)] private float limiteDroite = 1f;
    [SerializeField, Range(0.1f, 50f)] private float limiteGauche = 1f;
    private Vector3 _limiteDroitePosition;
    private Vector3 _limiteGauchePosition;
    private Rigidbody _rb;
    private float _direction = 1f;
    private SpriteRenderer _skin;

    [Header("FollowPlayerPatrol")]
    [SerializeField] float _attackBox;
    [SerializeField] float _attackRange;
    [SerializeField] float _attackRate = 1f;
    [SerializeField] float _nextAttackTime;

    private bool _facingLeft = true; //bool pour savoir ou regarde l'ennemi
    private bool _canSeePlayer = false;
    private bool _isDead = false;
    private Transform _player;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _skin = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _limiteDroitePosition = transform.position + new Vector3(limiteDroite, 0, 0);
        _limiteGauchePosition = transform.position - new Vector3(limiteGauche, 0, 0);
    }


    void Update()
    {
        float _distanceFromPlayer = Vector2.Distance(_player.position, transform.position);
        if (_distanceFromPlayer < _attackBox && !_isDead && _distanceFromPlayer > _attackRange) 
        {
            Debug.Log("true");
            _canSeePlayer = true;                                                                        
        }

        switch (_canSeePlayer)
        {
            case false:
                 // Si l'ennemi se coince contre quelque chose (sa vitesse plus petite que 0.1 m/s) alors il se retourne
                if (Mathf.Abs(_rb.velocity.x) < 0.1f)
                {
                    _direction = -_direction;
                    Flip();
                }
                

                //Si il dépasse sa limite Droite, il se retourne
                if (transform.position.x > _limiteDroitePosition.x || transform.position.x < _limiteGauchePosition.x)
                {
                    _direction = -_direction;
                    Flip();
                }
                

                _rb.velocity = new Vector2(_direction * _speed, _rb.velocity.y);
                break;

            case true:
                FlipTowardsPlayer();
                transform.position = Vector2.MoveTowards(this.transform.position, _player.position, _speed * Time.deltaTime);
                

                if (_distanceFromPlayer <= _attackRange && _nextAttackTime < Time.time && !_isDead) //sinon si l'ennemi est a portée pour tirer et n'est pas mort et peut tirer
                {
                    //Attack ou mort du joueur
                    Debug.Log("attack");
                    _nextAttackTime = Time.time + _attackRate;
                }
                break;
        }
    }

    void Flip()
    {
        _facingLeft = !_facingLeft; //on inverse la direction ou regarde l'ennemi
        transform.Rotate(0, 180, 0);      
    }

    void FlipTowardsPlayer()
    {
        float _playerDirection = _player.position.x - transform.position.x;
        if (_playerDirection > 0 && _facingLeft)
        {
            Flip();
        }
        else if (_playerDirection < 0 && !_facingLeft)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Damage
    }

    void OnDrawGizmos() //tracés dans l'editor
    {
        if (!Application.IsPlaying(gameObject))
        {
            _limiteDroitePosition = transform.position + new Vector3(limiteDroite, 0, 0);
            _limiteGauchePosition = transform.position - new Vector3(limiteGauche, 0, 0);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawCube(_limiteDroitePosition, new Vector3(0.2f, 1, 0.2f));
        Gizmos.DrawCube(_limiteGauchePosition, new Vector3(0.2f, 1, 0.2f));
        Gizmos.DrawLine(_limiteDroitePosition, _limiteGauchePosition);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackBox);
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}