import { Box, CircularProgress } from "@mui/material";

const LoadingComponent: React.FC = () => {
    return (
    <Box sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height:'100%'
    }}>
        <CircularProgress size="5rem" />
    </Box>
    );
}
export default LoadingComponent;