using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CursorMovement : Agent
{
    public bool clicked = false;

    const int nothing = 0;
    const int left = 1;
    const int right = 2;
    const int up = 3;
    const int down = 4;
    const int noclick = 0;
    const int click = 1;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(position.x, position.y);
        if (GameManager.instance.isGameOver)
        {
            EndEpisode();
        }
    }

    private void FixedUpdate()
    {
        //clicked = Input.GetMouseButton(0);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = Vector3.zero;
    }

    public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker)
    {
        //x : -5 ~ 4, y = -5 ~ 5

        var posX = transform.position.x;
        var posY = transform.position.y;
        if (posX <= -5)
        {
            actionMasker.SetMask(0, new[] { left });
        }
        if (posX >= 4)
        {
            actionMasker.SetMask(0, new[] { right });
        }
        if (posY <= -5)
        {
            actionMasker.SetMask(0, new[] { down });
        }
        if (posY >= 5)
        {
            actionMasker.SetMask(0, new[] { up });
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        SetReward(GameManager.instance.score / 100.0f);
        float action_movement = Mathf.FloorToInt(vectorAction[0]);
        float action_click = Mathf.FloorToInt(vectorAction[1]);
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        switch (action_movement)
        {
            case (nothing):
                break;
            case (left):
                transform.position = new Vector3(xPos - (1.0f / 16), yPos);
                break;
            case (right):
                transform.position = new Vector3(xPos + (1.0f / 16), yPos);
                break;
            case (up):
                transform.position = new Vector3(xPos, yPos + (1.0f / 16));
                break;
            case (down):
                transform.position = new Vector3(xPos, yPos - (1.0f / 16));
                break;
        }

        switch (action_click)
        {
            case (noclick):
                clicked = false;
                break;
            case (click):
                clicked = true;
                break;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actionsOut_ = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut_[0] = up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut_[0] = down;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut_[0] = right;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut_[0] = left;
        }
        else
        {
            actionsOut_[0] = nothing;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            actionsOut_[1] = click;
        }
        else
        {
            actionsOut_[1] = noclick;
        }
    }
}
