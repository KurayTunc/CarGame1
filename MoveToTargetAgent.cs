using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToTargetAgent : Agent
{
    //[SerializeField] private Transform environment; // �evre (environment) objesi
    [SerializeField] private Transform target; // Hedef obje
    [SerializeField] private Material rewardMaterial; // �d�l malzemesi
    [SerializeField] private Material penaltyMaterial; // Ceza malzemesi
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject ground;
    private Rigidbody rb;


    public override void Initialize() //awake gibi bu
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        // Hedefi ve ajan konumunu rastgele belirle
        target.localPosition = new Vector3(Random.Range(-4.4f, 4.665f), 0.22f, Random.Range(4.516f, -4.648f));
        transform.localPosition = new Vector3(Random.Range(0f, 4.665f), 0.22f, Random.Range(4.516f, -4.648f));

        // �evreyi d�nd�rme (iste�e ba�l�)
        // environment.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ajan�n konumunu ve hedef konumunu g�zlem olarak ekle
        sensor.AddObservation(transform.localPosition);
       // sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0]; // Sa�-Sol hareketi
        float moveForward = actions.ContinuousActions[1]; // �leri-Geri hareketi

        rb.MovePosition(transform.position + transform.forward * moveForward * movementSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * movementSpeed, 0f, Space.Self);



       /*// movementSpeed = 10f;

        // Hareket vekt�r�n� olu�tur
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * Time.deltaTime * movementSpeed;



        // Ajan�n konumunu g�ncelle
        transform.position += movement;*/
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        // Log detected inputs
        Debug.Log("Horizontal Input: " + Input.GetAxis("Horizontal"));
        Debug.Log("Vertical Input: " + Input.GetAxis("Vertical"));

        // Map keyboard inputs to continuous actions
        continuousActions[0] = Input.GetAxis("Horizontal"); // X axis (Right-Left)
        continuousActions[1] = Input.GetAxis("Vertical"); // Z axis (Forward-Backward)
        continuousActions[2] = 0; // No movement on Y axis (Up-Down)
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target")) // Hedef objesi etiketi ile kar��la�t�r
        {
            AddReward(5f);
            ChangeMaterial(rewardMaterial);
            EndEpisode();
        }
        else if (other.CompareTag("Wall")) // Duvar objesi etiketi ile kar��la�t�r
        {
            AddReward(-1f);
            ChangeMaterial(penaltyMaterial);
            EndEpisode();
        }
    }

    private void ChangeMaterial(Material material)
    {
        // �evre objesinin (veya ba�ka bir g�rsel ��enin) malzemesini de�i�tir
        ground.GetComponent<MeshRenderer>().material = material;
    }
}