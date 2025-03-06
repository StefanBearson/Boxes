import React from 'react';
import { BoxProps } from '../interfaces/BoxProps.ts';

interface GridProps {
    grid: BoxProps[][];
}

const Grid: React.FC<GridProps> = ({ grid }) => {
    return (
        <div className='flex justify-center min-h-screen'>
            {grid.map((row, rowIndex) => (
                <div key={rowIndex} className='flex-row'>
                    {row.map((box) => (
                        <div key={box.key} className='w-10 h-10 m-2 rounded-md border-2 border-[#1117] animate-scale-up' style={{ backgroundColor: box.color }}>
                        </div>
                    ))}
                </div>
            ))}
        </div>
    );
}

export default Grid;