export interface SnackbarProps {
    message: string;
    isVisible: boolean;
    onClose: () => void;
}