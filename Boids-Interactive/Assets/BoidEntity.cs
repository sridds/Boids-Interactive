using UnityEngine;

public class BoidEntity : MonoBehaviour
{
    private Vector3 velocity;
    private BoidSettings settings;

    public void Init(BoidSettings settings)
    {
        this.settings = settings;

        velocity = Random.insideUnitSphere * settings.minSpeed;
    }

    public void UpdateBoid(BoidEntity[] boids, int selfIndex)
    {
        Vector3 seperationVelocity = Seperation(boids, selfIndex);
        Vector3 alignmentVelocity = Alignment(boids, selfIndex);
        Vector3 cohesionVelocity = Cohesion(boids, selfIndex);

        // Calculate new velocity
        velocity += seperationVelocity;
        velocity += alignmentVelocity;
        velocity += cohesionVelocity;
        velocity = GetClampedVelocity(velocity);

        // bounce
        BounceOffEdges();

        // update position
        transform.position = transform.position + velocity * Time.deltaTime;

        // update rotation
        Quaternion targetRot = Quaternion.LookRotation(velocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * settings.rotationSpeed);
    }

    private void BounceOffEdges()
    {
        // outside of the top boundary
        if (transform.position.y > settings.boidBounds.y / 2.0f)
        {
            velocity = Vector3.Reflect(velocity, Vector3.down);
            //transform.position = new Vector3(transform.position.x, -settings.boidBounds.y / 2.0f, transform.position.z);
        }

        // outside of the bottom boundary
        if (transform.position.y < -settings.boidBounds.y / 2.0f)
        {
            velocity = Vector3.Reflect(velocity, Vector3.up);
            //transform.position = new Vector3(transform.position.x, settings.boidBounds.y / 2.0f, transform.position.z);
        }

        // outside of the top boundary
        if (transform.position.z > settings.boidBounds.z / 2.0f)
        {
            velocity = Vector3.Reflect(velocity, -Vector3.forward);
            //transform.position = new Vector3(transform.position.x, transform.position.y, -settings.boidBounds.z / 2.0f);
        }

        // outside of the bottom boundary
        if (transform.position.z < -settings.boidBounds.z / 2.0f)
        {
            velocity = Vector3.Reflect(velocity, Vector3.forward);
            //transform.position = new Vector3(transform.position.x, transform.position.y, settings.boidBounds.z / 2.0f);
        }

        // outside of the right boundary
        if (transform.position.x > settings.boidBounds.x / 2.0f)
        {
            velocity = Vector3.Reflect(velocity, -Vector3.right);
            //transform.position = new Vector3(-settings.boidBounds.x / 2.0f, transform.position.y, transform.position.z);
        }

        // outside of the left boundary
        if (transform.position.x < -settings.boidBounds.x / 2.0f)
        {
            velocity = Vector3.Reflect(velocity, Vector3.right);
            //transform.position = new Vector3(settings.boidBounds.x / 2.0f, transform.position.y, transform.position.z);
        }
    }

    private Vector3 GetClampedVelocity(Vector3 curVel)
    {
        float curSpeed = curVel.magnitude;
        curSpeed = Mathf.Clamp(curSpeed, settings.minSpeed, settings.maxSpeed);

        return curVel.normalized * curSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, velocity.normalized * 2.0f);
    }

    private Vector3 Seperation(BoidEntity[] boids, int selfIndex)
    {
        Vector3 seperationVel = Vector3.zero;
        int boidCount = 0;

        for (int i = 0; i < boids.Length; i++)
        {
            // skip over self
            if (i == selfIndex) continue;

            Vector3 posSelf = boids[selfIndex].transform.position;
            Vector3 posOther = boids[i].transform.position;
            float dist = Vector3.Distance(posSelf, posOther);

            if (dist < settings.seperationRange)
            {
                Vector3 otherToSelf = posSelf - posOther;
                Vector3 dir = otherToSelf.normalized;
                Vector3 weightedVel = dir / dist;

                seperationVel += weightedVel;
                boidCount++;
            }
        }

        if (boidCount > 0)
        {
            seperationVel /= (float)boidCount;
            seperationVel *= settings.seperationK;
        }

        return seperationVel;
    }

    private Vector3 Alignment(BoidEntity[] boids, int selfIndex)
    {
        Vector3 alignmentVel = Vector3.zero;
        int boidCount = 0;

        for(int i = 0; i < boids.Length; i++)
        {
            Vector3 posSelf = boids[selfIndex].transform.position;
            Vector3 posOther = boids[i].transform.position;
            float dist = Vector3.Distance(posSelf, posOther);

            if (dist < settings.alignmentRange)
            {
                alignmentVel += boids[i].velocity;
                boidCount++;
            }
        }

        if (boidCount > 0)
        {
            alignmentVel /= (float)boidCount;
            alignmentVel *= settings.alignmentK;
        }

        return alignmentVel;
    }

    private Vector3 Cohesion(BoidEntity[] boids, int selfIndex)
    {
        Vector3 centerOfMass = Vector3.zero;
        Vector3 cohesionVel = Vector3.zero;
        int boidCount = 0;

        for (int i = 0; i < boids.Length; i++)
        {
            if (i == selfIndex) continue;

            Vector3 posSelf = boids[selfIndex].transform.position;
            Vector3 posOther = boids[i].transform.position;
            float dist = Vector3.Distance(posSelf, posOther);

            if (dist < settings.cohesionRange)
            {
                centerOfMass += boids[i].transform.position;
                boidCount++;
            }
        }

        if(boidCount > 0)
        {
            centerOfMass /= (float)boidCount;
            cohesionVel = (centerOfMass - boids[selfIndex].transform.position) / (float)settings.cohesionRange;
            cohesionVel *= settings.cohesionK;
        }

        return cohesionVel;
    }
}
