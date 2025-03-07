import { useState } from 'react'
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

  const deleteSaveOnDb = async () => {
    await axios.delete("http://localhost:5052/box")
  }

  const loadGridFromDb = async () => {
    try {
      const { data } = await axios.get("http://localhost:5052/box");
      const newGrid: BoxProps[][] = [];
      data.forEach((box: BoxProps) => {
        if (!newGrid[box.y]) newGrid[box.y] = [];
        newGrid[box.y][box.x] = box;
      });
      setGrid(newGrid);

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
        setCurrentPosition({ x: last.currentPositionX, y: last.currentPositionY });
      } else {
        setRows(1);
        setIsNewLayer(false);
        setCurrentPosition({ x: 0, y: 0 });
      }
    } catch (error) {
      console.log(error);
    }
  }

  const postBox = async (box: BoxProps) => {
    try {
      setIsLoading(true);
      await axios.post("http://localhost:5052/box", box, {
        headers: { 'Content-Type': 'application/json' }
      });
    } catch (error) {
      console.log(error);
    } finally {
      setIsLoading(false);
    }
  }

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
      currentPositionX: x,
      currentPositionY: y
    };

    setGrid(prevGrid => {
      const newGrid = [...prevGrid];
      if (!newGrid[y]) newGrid[y] = [];
      newGrid[y][x] = box;
      return newGrid;
    });

    postBox(box);
  }

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

  const canMoveDown = (): boolean => {
    return rows > (currentPosition.x);
  }

  const handleAddBoxClick = () => {
    if(isLoading) return;
    setSnackbarData({isVisable: true, message: "Box added!"});
    theMagic();
  }

  const handleClearAllBoxesClick = () => {
    if(isLoading) return;
    deleteSaveOnDb();
    setGrid([])
    setCurrentPosition({x : 0, y:  0})
    setIsNewLayer(false)
    setRows(1)
    setSnackbarData({isVisable: true, message: "All boxes cleared!"});
  }

  const handleLoadLastSessionClick = () => {
    setSnackbarData({isVisable: true, message: "Last session loaded!"});
    loadGridFromDb();
  }

  const hideSnackbar = () => {setSnackbarData({isVisable: false, message: ""})}

  return (
    <div className='bg-purple-600 p-5'>
      <Buttons
          isLoading={isLoading}
          handleAddBoxClick={handleAddBoxClick}
          handleClearAllBoxesClick={handleClearAllBoxesClick}
          handleLoadLastSessionClick={handleLoadLastSessionClick}
      />
      <Grid grid={grid} />
      <Snackbar message={snackbarData.message} isVisible={snackbarData.isVisable} onClose={hideSnackbar} />
    </div>
  )
}

export default App;