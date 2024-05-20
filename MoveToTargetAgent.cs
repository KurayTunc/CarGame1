using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToTargetAgent : Agent
{
    //[SerializeField] private Transform environment; // Çevre (environment) objesi
    [SerializeField] private Transform target; // Hedef obje
    [SerializeField] private Material rewardMaterial; // Ödül malzemesi
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

        // Çevreyi döndürme (isteðe baðlý)
        // environment.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ajanýn konumunu ve hedef konumunu gözlem olarak ekle
        sensor.AddObservation(transform.localPosition);
       // sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0]; // Sað-Sol hareketi
        float moveForward = actions.ContinuousActions[1]; // Ýleri-Geri hareketi

        rb.MovePosition(transform.position + transform.forward * moveForward * movementSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * movementSpeed, 0f, Space.Self);



       /*// movementSpeed = 10f;

        // Hareket vektörünü oluþtur
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * Time.deltaTime * movementSpeed;



        // Ajanýn konumunu güncelle
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
        if (other.CompareTag("Target")) // Hedef objesi etiketi ile karþýlaþtýr
        {
            AddReward(5f);
            ChangeMaterial(rewardMaterial);
            EndEpisode();
        }
        else if (other.CompareTag("Wall")) // Duvar objesi etiketi ile karþýlaþtýr
        {
            AddReward(-1f);
            ChangeMaterial(penaltyMaterial);
            EndEpisode();
        }
    }

    private void ChangeMaterial(Material material)
    {
        // Çevre objesinin (veya baþka bir görsel öðenin) malzemesini deðiþtir
        ground.GetComponent<MeshRenderer>().material = material;
    }
}