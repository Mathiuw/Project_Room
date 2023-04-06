using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Vector3 maxRotaion;
    Vector3 rotation;
    [SerializeField] float speed;
    Transform cameraTransform;

    float intensity = 0;

    //Perlin noise seed
    float seedX;
    float seedY;
    float seedZ;

    //How much the shake increase/decrease
    [SerializeField] float growthIntensity = 1;
    [SerializeField] float decayIntensity = 1;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        seedX = Random.Range(-1000,1000);
        seedY = Random.Range(-1000,1000);
        seedZ = Random.Range(-1000,1000);
    }

    void Update()
    {
        //Debug Input
        //if (Input.GetKey(KeyCode.Space)) intensity += growthIntensity * Time.deltaTime;
        //else

        intensity -= decayIntensity * Time.deltaTime;
        intensity = Mathf.Clamp01(intensity);

        float intensityExponential = intensity * intensity;
        float time = Time.time * speed;

        rotation.x = intensityExponential * maxRotaion.x * PerlinNoise(seedX, time);
        rotation.y = intensityExponential * maxRotaion.y * PerlinNoise(seedY, time);
        rotation.z = intensityExponential * maxRotaion.z * PerlinNoise(seedZ, time);

        cameraTransform.localRotation = Quaternion.Euler(rotation);
    }

    float PerlinNoise(float x, float y) 
    {
        return Mathf.PerlinNoise(x,y);
    }

    public void AddCameraShake(float intensity, float speed) 
    {
        this.intensity = intensity;
        this.speed = speed;
    }

}
