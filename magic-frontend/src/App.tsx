import { useEffect, useState } from 'react'
import axios from 'axios'

import { getRandomColor } from './tools/randomColors'
import { BoxProps } from './interfaces/BoxProps.ts'
import Buttons from "./components/Buttons.tsx";
import Grid from "./components/Grid.tsx";
import Snackbar from "./components/Snackbar.tsx";

function App() {
  const [isNewLayer, setIsNewLayer] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [rows, setRows] = useState(1);
  const [currentPosition, setCurrentPosition] = useState({x : 0, y : 0});
  const [grid, setGrid] = useState<BoxProps[][]>([]);
  const [snackbarData, setSnackbarData] = useState({isVisable: false, message: ""});

  useEffect(()=>{
   loadGridFromDb();
  }, [])


  //Delete data from the API.
  const deleteSaveOnDb = async () => {
    try {
      await axios.delete("http://localhost:5052/box")
      setSnackbarData({isVisable: true, message: "All boxes cleared!"});
    } catch {
      setSnackbarData({isVisable: true, message: "No connection to the server!"});
    }
  }

  //Load data from the API.
  const loadGridFromDb = async () => {
    try {
      const { data } = await axios.get("http://localhost:5052/box");

      if (data.length === 0) {
        setSnackbarData({isVisable: true, message: "No data found on the server!"});
        return;
      }

      const newGrid: BoxProps[][] = [];
      
      data.forEach((box: BoxProps) => {
        if (!newGrid[box.y]) newGrid[box.y] = [];
        newGrid[box.y][box.x] = box;
      });

      setGrid(newGrid);
      setStartPosition(newGrid, data);
      setSnackbarData({isVisable: true, message: "Last session loaded!"});

    } catch (error) {
      setSnackbarData({isVisable: true, message: "No connection to the server!"});
      console.error(error);
    }
  }

  //Post box to API for persistent storage.
  const postBox = async (box: BoxProps) => {
    try {
      setIsLoading(true);
      await axios.post("http://localhost:5052/box", box, {
        headers: { 'Content-Type': 'application/json' }
      });
      setSnackbarData({isVisable: true, message: "Box added!"});
    } catch (error) {
      setSnackbarData({isVisable: true, message: "Box added! But not saved because no connection to the server!"});
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  }

  /**
   * Sets the starting position for a new box in the grid after loading last session from the server.
   *
   * @param {BoxProps[][]} newGrid - The current state of the grid.
   * @param {BoxProps[]} data - The array of box properties.
   */
  const setStartPosition = (newGrid: BoxProps[][],  data: BoxProps[]) => {
    const last = data.sort((a: BoxProps, b: BoxProps) => b.key - a.key)[0];
    const highestX = Math.max(...data.map((box: BoxProps) => box.x));
    const numberOfRows = newGrid.length;
    const numberOfCols = newGrid[0].length;
    const numberOfColsInLastRow = newGrid[numberOfRows - 1].length;
    const shouldBeNewLayer = numberOfCols === numberOfRows && numberOfColsInLastRow === numberOfRows;
    const isOnLastRow = last.x === numberOfCols;

    if (last) {
      setRows(isOnLastRow ? highestX : highestX + 1);
      setIsNewLayer(shouldBeNewLayer);
      setCurrentPosition({ x: last.x, y: last.y });
    } else {
      setRows(1);
      setIsNewLayer(false);
      setCurrentPosition({ x: 0, y: 0 });
    }
  }

  /**
   * Places a box on the grid at the specified (x, y) coordinates.
   * 
   * @param x - The x-coordinate where the box should be placed.
   * @param y - The y-coordinate where the box should be placed.
   * 
   * This function updates the current position state, determines if a new row
   * should be added, and creates a new box with a random color. It then updates
   * the grid state with the new box and posts the box data to a server
   * 
   */
  const placeBoxOnXY = (x: number, y: number) => {
    setCurrentPosition({ x, y });

    setRows(y === 0 ? x + 1 : rows);
    setIsNewLayer(y === 0);

    const box: BoxProps = {
      key: grid.flat().length,
      x,
      y,
      color: getRandomColor(),
      row: rows,
      isNewLayer,
    };

    setGrid(prevGrid => {
      const newGrid = [...prevGrid];
      if (!newGrid[y]) newGrid[y] = [];
      newGrid[y][x] = box;
      return newGrid;
    });

    postBox(box);
  }

  /**
   * Handles the logic for placing a box on the grid based on the current state.
   * 
   * - If the grid is empty, it clears the saved session, shows a snackbar message,
   *   and places the first box at position (0, 0).
   * - If a new layer is being added, it places the box at the end of the grid.
   * - If the box can move down, it moves the box down by one position.
   * - Otherwise, it moves the box left by one position.
   */
  const theMagic = () => {
    if (!grid.length) {
      deleteSaveOnDb();
      setSnackbarData({isVisable: true, message: "First box added! Your saved Session is cleared"});
      placeBoxOnXY(0, 0);
    } else if (isNewLayer) {
      placeBoxOnXY(0, grid.length);
    } else if (canMoveDown()) {
      placeBoxOnXY(currentPosition.x + 1, currentPosition.y);
    } else {
      placeBoxOnXY(currentPosition.x, currentPosition.y - 1);
    }
  }

  //helper functions that check whether it can be placed downwards.
  const canMoveDown = (): boolean => {
    return rows > (currentPosition.x);
  }

  const handleAddBoxClick = () => {
    if(isLoading) return;
    theMagic();
  }

  const handleClearAllBoxesClick = () => {
    if(isLoading) return;
    deleteSaveOnDb();
    setGrid([])
    setCurrentPosition({x : 0, y:  0})
    setIsNewLayer(false)
    setRows(1)
  }

  const hideSnackbar = () => {setSnackbarData({isVisable: false, message: ""})}

  return (
    <div className='bg-purple-600 p-5'>
      <Buttons
          isLoading={isLoading}
          handleAddBoxClick={handleAddBoxClick}
          handleClearAllBoxesClick={handleClearAllBoxesClick}
      />
      <Grid grid={grid} />
      <Snackbar message={snackbarData.message} isVisible={snackbarData.isVisable} onClose={hideSnackbar} />
    </div>
  )
}

export default App;