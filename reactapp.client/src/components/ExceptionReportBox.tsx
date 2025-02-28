import { Box, Paper, Typography } from "@mui/material"

const ExceptionReportBox: React.FC<{ infoMessage: string }> = ({ infoMessage }) => {
    return (
        <Box width={'100%'}>
            <Paper
                elevation={3}
            >
                <Typography textAlign={'center'} color="info" padding={5}>
                    {infoMessage}
                </Typography>

            </Paper>
        </Box>

    )
}

export default ExceptionReportBox;