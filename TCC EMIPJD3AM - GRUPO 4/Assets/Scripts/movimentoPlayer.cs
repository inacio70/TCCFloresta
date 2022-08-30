using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimentoPlayer : MonoBehaviour
{
    [Header("Controle Personagem")]
    private Transform cam;
    private CharacterController controller;
    private Animator anim;
    public float velocidadeMovimento;
    private Vector3 direcao;
    private Vector3 direcaoMovimento; 
    private float suavizacaoMovimento = 0.01f;
    private float suavizancaoVelocidade;
    private bool walk;


    [Header("Controle Gravidade")]
    public Transform gruondCheck;
    public LayerMask layerInteracao;
    public float gravidade;
    public bool sensor;
    private Vector3 velocidade;
    public float alturaPulo;


    public bool dormir;



    void Start()
    {
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputplayer();
        MovimentoPersonagem();

        anim.SetBool("ground", sensor);
    }

    private void FixedUpdate()
    {
        sensor = Physics.CheckSphere(gruondCheck.position, 0.3f, layerInteracao);
    }


    void Inputplayer()
    {

        direcao = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (Input.GetButtonDown("Jump") && sensor)
        {

            velocidade.y = Mathf.Sqrt(alturaPulo * -2 * gravidade);
        }


    }
    void MovimentoPersonagem()
    {

        if (sensor && velocidade.y < 0)
        {
            velocidade.y = -0.01f;
        }


        if (direcao.magnitude > 0.1f)
        {
            float anguloDestino = Mathf.Atan2(direcao.x, direcao.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloDestino, ref suavizancaoVelocidade, suavizacaoMovimento);

            transform.rotation = Quaternion.Euler(0f, angulo, 0f);

            direcaoMovimento = Quaternion.Euler(0f, anguloDestino, 0f) * Vector3.forward;

            walk = true;


        }
        else
        {
            walk = false;
        }


        controller.Move(direcaoMovimento.normalized * velocidadeMovimento * direcao.magnitude * Time.deltaTime);

        anim.SetBool("walk", walk);

        velocidade.y += gravidade * Time.deltaTime;

        controller.Move(velocidade * Time.deltaTime);

     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "dormir")
        {
            if (Input.GetKey(KeyCode.Z))
            {
                anim.SetBool("dormir", true);
            }

           
        }
    }

    

}
