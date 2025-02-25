import { Skeleton, styled } from "@mui/material";
import Grid from "@mui/material/Grid2";
// const Skeleton = styled('div')<{ height: number }>(({ theme, height }) => ({
//     backgroundColor: theme.palette.action.hover,
//     borderRadius: theme.shape.borderRadius,
//     height,
//     content: '" "',
// }));

const PlaceHolder: React.FC = () => {
    return <>
        <Grid margin={3} container spacing={1}>
            <Grid size={5} />
            <Grid size={12}>
                <Skeleton animation="wave" height={14} />
            </Grid>
            <Grid size={12}>
                <Skeleton animation="wave" height={14} />
            </Grid>
            <Grid size={4}>
                <Skeleton animation="wave" height={100} />
            </Grid>
            <Grid size={8}>
                <Skeleton animation="wave" height={100} />
            </Grid>

            <Grid size={12}>
                <Skeleton animation="wave" height={300} />
            </Grid>
            <Grid size={12}>
                <Skeleton animation="wave" height={100} />
            </Grid>

            <Grid size={3}>
                <Skeleton animation="wave" height={100} />
            </Grid>
            <Grid size={3}>
                <Skeleton animation="wave" height={100} />
            </Grid>
            <Grid size={3}>
                <Skeleton animation="wave" height={100} />
            </Grid>
            <Grid size={3}>
                <Skeleton animation="wave" height={100} />
            </Grid>
        </Grid>
    </>
}

export default PlaceHolder;