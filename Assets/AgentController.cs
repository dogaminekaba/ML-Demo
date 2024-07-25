using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;

public class AgentController : Agent
{
    public GameObject target;
    public float forceMultiplier = 0.1f;
    public GameController gameManager;

    private Rigidbody rBody;
    private EnemyController targetController;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        targetController = target.GetComponent<EnemyController>();
    }
    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.25f, 0);
        }

        // Move the target to a new spot
        targetController.ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.transform.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        //rBody.AddForce(controlSignal * forceMultiplier);
        transform.position += controlSignal * forceMultiplier;

        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            SetReward(0.0f);
            EndEpisode();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Assign a negative reward when colliding with a bullet
            SetReward(0.0f);
            Destroy(other.gameObject);
            EndEpisode();

            gameManager.IncreaseLoseCount();
        }
        else if (other.gameObject.CompareTag("Target"))
        {
            // Assign a positive reward when colliding with the target
            SetReward(1.0f);
            EndEpisode();

            gameManager.IncreaseWinCount();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
