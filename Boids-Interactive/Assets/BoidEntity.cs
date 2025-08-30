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
        Vector3 accel = Vector3.zero;

        Vector3 seperationForce = Seperation(boids, selfIndex);
        Vector3 alignmentForce = Alignment(boids, selfIndex);
        Vector3 cohesionForce = Cohesion(boids, selfIndex);

        // Calculate new velocity
        accel += seperationForce;
        accel += alignmentForce;
        accel += cohesionForce;

        if(IsOnCollisionCourse())
        {
            Vector3 collisionAvoidance;
            Vector3 avoidanceForce;

            //accel += avoidanceForce;
        }

        velocity += accel * Time.deltaTime;
        velocity = GetClampedVelocity(velocity);

        // update position
        transform.position = transform.position + velocity * Time.deltaTime;

        // update rotation
        Quaternion targetRot = Quaternion.LookRotation(velocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * settings.rotationSpeed);
    }

    private bool IsOnCollisionCourse()
    {
        RaycastHit hit;
        if(Physics.SphereCast(transform.position, 2.0f, velocity.normalized, out hit, 1.0f))
        {
            return true;
        }
        return false;
    }

    private Vector3 GetAvoidanceDirection()
    {
        /*
        Vector3[] rays;

        for(int i = 0; i < rays.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rays[i]);
            Ray ray = new Ray(transform.position, dir);

            if(!Physics.SphereCast(ray, 2.0f, 1.0f))
            {
                return dir;
            }
        }

        return Vector3.forward;*/
        return Vector3.zero;
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
