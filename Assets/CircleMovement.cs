using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class CircleMovement : MonoBehaviour
{
    Camera Cam;

    //Ball Variables
    UnityEngine.Vector3 BallPosition;
    public UnityEngine.Vector3 BallVelocity;
    public UnityEngine.Vector3 BallAccleration;
    int maxVelocity = 15;

    //Mouse Variables
    UnityEngine.Vector3 MouseLocation;
    UnityEngine.Vector3 MouseCamLocation;
    
    //Black Hole Variables
    GameObject blackhole;
    UnityEngine.Vector3 BlackHolePosition;
    float X_Distance,Y_Distance,Distance;
    public float FroceStrength = 3000f;

    //Pid Variables
    public float Kp=0.05f;
    public float Ki=0.0005f;
    public float Kd=0.5f;
    float ErrorX, ErrorY, PreviousErrorX, PreviousErrorY;
    float Ix,Dx,Iy,Dy;

    void Start()
    {
        //Gets the Camera object
        Cam = Camera.main;

        //Gets the object spawned from the Spawner class
        Spawner spawner = FindObjectOfType<Spawner>();
        blackhole = spawner.getGameObject(); 
    }

    void Update()
    {
        //Gets the mouse location from the camera's pov
        MouseLocation = Input.mousePosition;
        MouseCamLocation = Cam.ScreenToWorldPoint(MouseLocation);
        MouseCamLocation[2] = 0;

        //Updates the position of the ball and the black hole
        BallPosition = transform.position;
        BlackHolePosition = blackhole.transform.position;

        //pid();
        //blackHolePid();
        blackHoleForce();
        moveBall();
        resetBall();
        //updateVelocity();
        VelocityLimit();

        //Used to input the balls velocity
        transform.Translate(BallVelocity[0],BallVelocity[1],0f);

        //Used if you are using the pid
        PreviousErrorX = ErrorX;
        PreviousErrorY = ErrorY;
    }

    public void updateVelocity()
    {
        BallVelocity[0] += BallAccleration[0];
        BallVelocity[1] += BallAccleration[1];
    }
    public void mousePid()
    {
        ErrorX = MouseCamLocation[0]-BallPosition[0];
        ErrorY = MouseCamLocation[1]-BallPosition[1];
        Ix += ErrorX;
        Iy += ErrorY;
        Dx = ErrorX-PreviousErrorX;
        Dy = ErrorY-PreviousErrorY;
        BallVelocity[0]=Kp*ErrorX + Ki*Ix + Kd*Dx;
        BallVelocity[1]=Kp*ErrorY + Ki*Iy + Kd*Dy;
    }

    public void blackHolePid()
    {
        ErrorX = BlackHolePosition[0]-BallPosition[0];
        ErrorY = BlackHolePosition[1]-BallPosition[1];
        Ix += ErrorX;
        Iy += ErrorY;
        Dx = ErrorX-PreviousErrorX;
        Dy = ErrorY-PreviousErrorY;
        BallVelocity[0]=Kp*ErrorX + Ki*Ix + Kd*Dx;
        BallVelocity[1]=Kp*ErrorY + Ki*Iy + Kd*Dy;
    }

    public void blackHoleForce()
    {
        X_Distance = BlackHolePosition[0]-BallPosition[0];
        Y_Distance = BlackHolePosition[1]-BallPosition[1];
        Distance = (float)Math.Sqrt(Math.Pow(X_Distance,2) + Math.Pow(Y_Distance,2));
        float force = FroceStrength/(Distance*Distance);
        float forcex = force*(X_Distance/Distance);
        float forcey = force*(Y_Distance/Distance);
        BallVelocity[0]+=forcex;
        BallVelocity[1]+=forcey;
    }

    public void moveBall()
    {
        if(Input.GetKey("w"))
        {
            BallVelocity[1] += 0.1f;
        }
        if(Input.GetKey("a"))
        {
            BallVelocity[0] += -0.1f;
        }
        if(Input.GetKey("s"))
        {
            BallVelocity[1] += -0.1f;
        }
        if(Input.GetKey("d"))
        {
            BallVelocity[0] += 0.1f;
        }
    }

    public void resetBall()
    {
        if(Input.GetKey("r"))
        {
            BallPosition[0] = 0f;
            BallPosition[1] =950f;
            BallPosition[2] = 0f;
            BallVelocity[0]=0f;
            BallVelocity[1]=0f;
            BallVelocity[2]=0f;
            transform.position = BallPosition;
        }
    }

    public void VelocityLimit()
    {
        if(BallVelocity[0]>maxVelocity)
        {
            BallVelocity[0]=maxVelocity;
        }
        if(BallVelocity[0]<-maxVelocity)
        {
            BallVelocity[0]=-maxVelocity;
        }
        if(BallVelocity[1]>maxVelocity)
        {
            BallVelocity[1]=maxVelocity;
        }
        if(BallVelocity[1]<-maxVelocity)
        {
            BallVelocity[1]=-maxVelocity;
        }
    }

}
