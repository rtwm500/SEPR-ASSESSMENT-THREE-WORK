﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    CLASS: MapCamera
    FUNCTION: Controls movement of the camera
 */

public class MapCamera : MonoBehaviour {

    // The min and max zoom positions
	public float minZoom, maxZoom;
    // The movement speed of the camera
    public float moveSpeed;
    // The two cameras
    public Camera cam1, cam2;

    // The current zoom
	float zoom = 1f;

    // Map width
	private const float WIDTH = 24.0f;
    // Map height
	private const float HEIGHT = 13.0f;
    // The size of a tile
	private const float TILE_SIZE = 0.75f;

    // Maximum x position
	private float maxX = WIDTH * TILE_SIZE + TILE_SIZE / 2;
    // Maximum y position
	private float maxY = HEIGHT * TILE_SIZE - TILE_SIZE / 2;
    // Minimum x position
	private float minX = -TILE_SIZE / 2;
    // Minimum y position
	private float minY = TILE_SIZE / 2;

    void Start()
    {
        // Set camera 1 as the starting camera
        cam1.enabled = true;
        cam2.enabled = false;
    }

    void Update ()
    {
        // Get the amount to zoom by
		float zoomDelta = Input.GetAxis("Mouse ScrollWheel");

        // Get the amount to move the camera in the x and y directions
        float xDelta = Input.GetAxis("Horizontal");
        float yDelta = Input.GetAxis("Vertical");

        // If we need to zoom, call AjdustZoom
		if (zoomDelta != 0f) {
			AdjustZoom(zoomDelta);
		}

        // If we need to move, call AdjustPosition
        if (xDelta != 0f || yDelta != 0f)
        {
            AdjustPosition(xDelta, yDelta);
        }

        // If 'C' is pressed, swap between camera 1 and camera 2
        if (Input.GetKeyDown(KeyCode.C))
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
        }
	} 

	public void centerPos(int[] coords){ 
		SetPosition (coords[0], coords[1]);
	}


	/// <summary>
	/// Adjusts the zoom of the cmaera by changing its orthographic size.
	/// </summary>
	/// <param name="delta">Delta - change in zoom value.</param>
	void AdjustZoom(float delta)
	{
        // Set the zoom value, clamped between 0 and 1
		zoom = Mathf.Clamp01(zoom + delta);
        
        // Calculate the new orthographic size by lerping between the max and min zoom by a factor of the zoom value
		float distance = Mathf.Lerp(maxZoom, minZoom, zoom);
        // Set the new orthographic size
		cam1.orthographicSize = distance;
	}

	/// <summary>
	/// Adjusts the position of the camera.
	/// </summary>
	/// <param name="xDelta">X delta - change in x value.</param>
	/// <param name="yDelta">Y delta - change in y value.</param>
    void AdjustPosition(float xDelta, float yDelta)
    {
        Vector2 direction = new Vector2(xDelta, yDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));
        float distance = moveSpeed * damping * Time.deltaTime;

        Vector2 position = transform.localPosition;
		position.x = Mathf.Clamp(position.x + direction.x * distance, minX, maxX);
		position.y = Mathf.Clamp(position.y + direction.y * distance, maxY, minY);

		transform.localPosition = position;     
    } 

	void SetPosition(float x, float y)
	{    
		Vector2 position = transform.localPosition;
		position.x = Mathf.Clamp(x - cam1.orthographicSize, minX, maxX);
		position.y = Mathf.Clamp(-y + cam1.orthographicSize, maxY, minY);
		transform.localPosition = position;    
	} 


	/// <summary>
	/// Limits the maximum coordinates.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
    public void SetMaxCoord(float x, float y){
        maxX = x;
        maxY = -y;
    }
}
