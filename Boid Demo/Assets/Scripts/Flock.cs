using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Flock
    [Header("Flock Setup")]
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    // Initial Values
    [Header("Settings of Flock")]
    [Range(10, 500)]
    public int startingAmount;
    const float AgentDensity = 0.4f;
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed;
    [Range(1f, 100f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;

    // Get Methods
    public float  SquareAvoidanceRadius {get {return squareAvoidanceRadius;}}

    // Start is called before the first frame update
    void Start()
    {
        // Initialization
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        // Spawn Initial Flock
        for(int i = 0; i < startingAmount; i++)
        {
            FlockAgent newAgent = Instantiate(agentPrefab, Random.insideUnitCircle * startingAmount * AgentDensity, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Update all agent target positions
        // this loop will iterate thourgh all agents and tell them to move -
        // - based on near by agents
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearByObjects(agent);
            // This is for Debugging ONLY
            // agent.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if(move.sqrMagnitude > squareMaxSpeed)
                move = move.normalized * maxSpeed;
            // Tell agent to move
            agent.Move(move);
        }
    }

    // Find All Near Objects
    // returns a list of all nearby agent transforms
    List<Transform> GetNearByObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[]  contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            if(c != agent.AgentCollider)
                context.Add(c.transform);
        }
        return context;
    }
}
