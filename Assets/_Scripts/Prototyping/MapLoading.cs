using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapLoading : MonoBehaviour {

    // File name to load
    public string m_mapFileName;    // NOTE: We may need to do this a different way when loading many maps (dictionary perhaps)
    // Reference to the grid class
    private AI.Grid m_grid;

	void Start ()
    {
        m_grid = GetComponent<AI.Grid>();
        LoadMap(m_mapFileName);
	}
	
	void Update ()
    {
		
	}

    // Load map code from a text file
    private bool LoadMap(string fileName)
    {
        string line;
        line = string.Join("", File.ReadAllLines(Application.dataPath + "/" + fileName));   // Store the text file as 1 string, replace new lines with ""
        if(line != null)                        // If there is somthing in the file
        {
            string[] data = line.Split('_');    // Split the file at all the "_"    
                                                // This creates an array of strings
            if (data.Length > 0)                
            {
                                                // The array of strings is passed on to build the map
                BuildMap(data);
            }
            else
            {
                Debug.LogError("Reading map file error.  File name: " + fileName);
                return false;
            }
        }
        else
        {
            Debug.LogError("Nothing in the map file. File name: " + fileName);
            return false;
        }

        return true;
    }

    // Build the actual grid
    private void BuildMap(string[] data)
    {
        int width;
        int height;
        // The first string in the array should be the width
        // NOTE: TryParse tries to convert the string into an int
        if(!int.TryParse(data[0], out width))
        {
            Debug.LogError("Width not read corretly.");
        }
        // The second string in the array should be the height
        if(!int.TryParse(data[1], out height))
        {
            Debug.LogError("Height not read corretly.");
        }

        // Create the grid with the given dimensions
        m_grid.CreateNewGrid(width, height);

        // The third srting in the array gets split futher into a new string array at every ","
        string[] mapData = data[2].Split(',');
        if (mapData.Length > 0)
        {
            // Go through the whole grid that was created
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    AI.ENodeTypes newType = AI.ENodeTypes.Floor;
                    // Grab the data from the map data to see what type of tile is needed at the current coordinate
                    switch (mapData[y * width + x])
                    {
                        case "f":
                            newType = AI.ENodeTypes.Floor;
                            break;
                        case "w":
                            newType = AI.ENodeTypes.Water;
                            break;
                        case "-":
                            newType = AI.ENodeTypes.Wall;
                            break;
                        default:
                            newType = AI.ENodeTypes.Floor;
                            break;
                    }

                    // Set the tile to the type read from the map file
                    m_grid.SetGridTile(x, y, newType);
                }
            }
        }
        else
        {
            Debug.LogError("No data in map file!");
        }
    }
}
