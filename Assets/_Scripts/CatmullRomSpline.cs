using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSpline : MonoBehaviour
{
	//Has to be at least 2 points << modify 
	public Vector3[] controlPointsList; //input passed in from VR Controller
										//Are we making a line or a loop?
	public bool isLooping = true;
	//how many divisions per segment
	public int divisions = 2;
	public int knotCount = 0;

	//-------------PRIVATE--------------------------
	private Vector3[] knotLocations;
	private Vector3[] knotTangents;


	public Vector3[] getKnotLocations()
	{
		return knotLocations;
	}

	public Vector3[] getKnotTangents()
	{
		return knotTangents;
	}

	//Display without having to press play
	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;

		QueryResults();

		//Draw the Catmull-Rom spline between the points
		/*
		for (int i = 0; i < controlPointsList.Length - 1; i++)
		{
			DisplayCatmullRomSpline(i);
		}
		*/
	}

	//Display a spline between 2 points derived with the Catmull-Rom spline algorithm
	private void DisplayCatmullRomSpline(int pos)
	{
		Vector3 p0, p1, p2, p3 = new Vector3();
		if (isLooping)
		{
			//The 4 points we need to form a spline between p1 and p2
			p0 = controlPointsList[ClampListPos(pos - 1)];
			p1 = controlPointsList[pos];
			p2 = controlPointsList[ClampListPos(pos + 1)];
			p3 = controlPointsList[ClampListPos(pos + 2)];
		}
		else //is not looping, the do the casin on pos = first or last.
		{
			//The 4 points we need to form a spline between p1 and p2
			//case on if pos = 0 (first index, need to complete one before)
			p1 = controlPointsList[pos];
			p2 = controlPointsList[pos + 1];



			if (pos - 1 < 0) //pos = first point
			{
				p0 = p1 - (p2 - p1);
			}
			else
			{ //else query as ususal
				p0 = controlPointsList[pos - 1];
			}

			if (pos + 2 == controlPointsList.Length) //pos =  second to last point??
			{
				p3 = p2 + (p2 - p1);
			}
			else
			{
				p3 = controlPointsList[pos + 2];
			}
		}

		//The start position of the line
		Vector3 lastPos = p1;

		//The spline's resolution
		//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
		float resolution = 1.0f / (float)divisions;

		for (int i = 1; i <= divisions; i++) //NOTE: less than or equal purely for displaying
		{
			//Which t position are we at?
			float t = i * resolution;

			//Find the coordinate between the end points with a Catmull-Rom spline
			Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

			//Draw this line segment
			Gizmos.DrawLine(lastPos, newPos);

			//Save this pos so we can draw the next line segment
			lastPos = newPos;
		}
	}

	//Clamp the list positions to allow looping
	private int ClampListPos(int pos)
	{
		if (pos < 0)
		{
			pos = controlPointsList.Length - 1;
		}

		if (pos > controlPointsList.Length)
		{
			pos = 1;
		}
		else if (pos > controlPointsList.Length - 1)
		{
			pos = 0;
		}

		return pos;
	}


	//Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
	//http://www.iquilezles.org/www/articles/minispline/minispline.htm
	private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
		Vector3 a = 2f * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

		//The cubic polynomial: a + b * t + c * t^2 + d * t^3
		Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

		return pos;
	}

	//clean data version of Display G
	private void QueryPointData(int pos)
	{
		Vector3 p0, p1, p2, p3 = new Vector3();
		if (isLooping)
		{
			//The 4 points we need to form a spline between p1 and p2
			p0 = controlPointsList[ClampListPos(pos - 1)];
			p1 = controlPointsList[pos];
			p2 = controlPointsList[ClampListPos(pos + 1)];
			p3 = controlPointsList[ClampListPos(pos + 2)];
		}
		else //is not looping, the do the casin on pos = first or last.
		{
			//The 4 points we need to form a spline between p1 and p2
			//case on if pos = 0 (first index, need to complete one before)
			p1 = controlPointsList[pos];
			p2 = controlPointsList[pos + 1];

			//edge cases
			if (pos - 1 < 0) //pos = first point
			{
				p0 = p1 - (p2 - p1);
			}
			else
			{ //else query as ususal
				p0 = controlPointsList[pos - 1];
			}

			if (pos + 2 == controlPointsList.Length) //pos =  second to last point??
			{
				p3 = p2 + (p2 - p1);
			}
			else
			{
				p3 = controlPointsList[pos + 2];
			}
		}
		// The start position of the line
		Vector3 lastPos = p1;

		//The spline's resolution
		//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
		float resolution = 1.0f / (float)divisions;

		//load the first point
		knotLocations[pos * divisions] = lastPos;

		for (int i = 1; i <= divisions; i++) //NOTE: less than or equal purely for displaying
		{
			//Which t position are we at?
			float t = i * resolution;

			//Find the coordinate between the end points with a Catmull-Rom spline
			Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

			//push into the data strucutre 
			knotLocations[pos * divisions + i] = newPos;
			knotTangents[pos * divisions + i] = (newPos - lastPos).normalized;

			//Save this pos so we can draw the next line segment
			lastPos = newPos;
		}
	}

	public RawMesh QueryResults()
	{

		//calculate how many points we will gave
		knotCount = divisions * (controlPointsList.Length - 1) + 1;
		knotLocations = new Vector3[knotCount]; //last point doesn't have tangent
		knotTangents = new Vector3[knotCount];


		for (int i = 0; i < controlPointsList.Length - 1; i++)
		{
			QueryPointData(i);
		}
		//add the last point so I can debug it
		knotLocations[knotCount - 1] = controlPointsList[controlPointsList.Length - 1];


		RawMesh ret = new RawMesh();
		ret.knotLoc = knotLocations;
		ret.knotTan = knotTangents;
		ret.knotCount = knotCount;

		return ret;
	}

}
