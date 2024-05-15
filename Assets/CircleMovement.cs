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

    //Mouse Variables
    UnityEngine.Vector3 MouseLocation;
    UnityEngine.Vector3 MouseCamLocation;
    
    //Black Hole Variables
    GameObject blackhole;
    UnityEngine.Vector3 BlackHolePosition;
    float X_Distance,Y_Distance,Distance;
    public float FroceStrength = 3000f;

    //Pid Variables
    float Kp=0.01f;
    float Ki=0.0005f;
    float Kd=0.1f;
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
        updateVelocity();
        maxVelocity();

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
    public void pid()
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
        BallAccleration[0]=Kp*ErrorX + Ki*Ix + Kd*Dx;
        BallAccleration[1]=Kp*ErrorY + Ki*Iy + Kd*Dy;
    }

    public void blackHoleForce()
    {
        X_Distance = BlackHolePosition[0]-BallPosition[0];
        Y_Distance = BlackHolePosition[1]-BallPosition[1];
        Distance = (float)Math.Sqrt(Math.Pow(X_Distance,2) + Math.Pow(Y_Distance,2));
        float force = FroceStrength/(Distance*Distance);
        float forcex = force*(X_Distance/Distance);
        float forcey = force*(Y_Distance/Distance);
        BallAccleration[0]=forcex;
        BallAccleration[1]=forcey;
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

    public void maxVelocity()
    {
        if(BallVelocity[0]>15)
        {
            BallVelocity[0]=15;
        }
        if(BallVelocity[0]<-15)
        {
            BallVelocity[0]=-15;
        }
        if(BallVelocity[1]>15)
        {
            BallVelocity[1]=15;
        }
        if(BallVelocity[1]<-15)
        {
            BallVelocity[1]=-15;
        }
    }

}
