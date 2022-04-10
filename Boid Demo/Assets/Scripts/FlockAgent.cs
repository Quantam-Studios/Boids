using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{

    Flock agentFlock;
    public Flock AgentFlock {get {return agentFlock;}}

    // Sensory
    Collider2D agentCollider;
    public Collider2D AgentCollider {get {return agentCollider;}}

    public void Initialize(Flock flock)
    {
        agentFlock = flock; 
    }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
