import { useEffect } from 'react';
import { SnackbarProps } from '../interfaces/SnackBarProps';

const Snackbar: React.FC<SnackbarProps> = ({ message, isVisible, onClose }) => {
    useEffect(() => {
        if (isVisible) {
            const timer = setTimeout(onClose, 3000); // Auto-hide after 3 seconds
            return () => clearTimeout(timer);
        }
    }, [isVisible, onClose]);

    return (
        <div
            className={`z-40 fixed bottom-4 left-1/2 transform -translate-x-1/2 px-4 py-2 bg-gray-800 text-white rounded transition-opacity duration-300 ${
                isVisible ? 'opacity-100' : 'opacity-0'
            }`}
        >
            {message}
        </div>
    );
};

export default Snackbar;