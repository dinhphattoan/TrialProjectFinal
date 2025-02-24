import { Box, CircularProgress } from "@mui/material";

const LoadingComponent: React.FC = () => {
    return (
    <Box sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: 'white',
        width: '100vw',
        height: '100vh'
    }}>
        <CircularProgress size="5rem" />
    </Box>
    );
}
export default LoadingComponent;