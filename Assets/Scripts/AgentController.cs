using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentController : Agent
{
    public GameObject target;
    public float speedMultiplier;
    public GameController gameManager;

    private Rigidbody rBody;
    private TurretController targetController;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        targetController = target.GetComponent<TurretController>();
        animator = GetComponent<Animator>();
    }
    public override void OnEpisodeBegin()
    {
        animator.SetFloat("MoveSpeed", 1);
        animator.SetBool("Attack", true);

        // If the Agent fell, zero its momentum
        if (transform.localPosition.y < 0)
        {
            rBody.angularVelocity = Vector3.zero;
            rBody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0, 0, 0);
        }

        // Move the target to a new spot
        targetController.ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.transform.localPosition);
        sensor.AddObservation(transform.localPosition);

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

        transform.position += controlSignal * speedMultiplier;

        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                              transform.position.y,
                                              target.transform.position.z);

        transform.LookAt(targetPostition);

        // Fell off platform
        if (transform.localPosition.y < 0)
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
