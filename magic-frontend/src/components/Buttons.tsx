import {ButtonsProps} from "../interfaces/ButtonsProps.ts";

const Buttons = ({ isLoading, handleAddBoxClick, handleClearAllBoxesClick, handleLoadLastSessionClick }: ButtonsProps) => {
    return (
        <div className='flex justify-center gap-4 items-center'>
            <button onClick={handleAddBoxClick} disabled={isLoading} className={'px-4 py-2 text-white rounded ' + (isLoading ? 'bg-gray-500' : 'bg-green-500')}>Add Box</button>
            <button onClick={handleClearAllBoxesClick} disabled={isLoading} className='px-4 py-2 bg-red-500 text-white rounded'>Clear All Boxes</button>
            <button onClick={handleLoadLastSessionClick} disabled={isLoading} className='px-4 py-2 bg-purple-900 text-white rounded'>Load Last Session</button>
        </div>
    );
}

export default Buttons;