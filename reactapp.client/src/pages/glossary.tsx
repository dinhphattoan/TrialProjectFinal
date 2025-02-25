import { useActivePage } from "@toolpad/core"
import React, { useEffect } from "react";
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import { Box, Button, ButtonGroup, Divider, Icon, IconButton, Stack, TextField, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";
import Grid from '@mui/material/Grid2';
import CreateIcon from '@mui/icons-material/Create';
import SkipPreviousIcon from '@mui/icons-material/SkipPrevious';
import SkipNextIcon from '@mui/icons-material/SkipNext';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
interface TemplateProp {
    children: React.ReactElement,

}


const PageTemplate: React.FC<TemplateProp> = () => {
    const handleAddGlossary = async () => {
        console.log("click!");
    }
    const colWidth = { xs: 12, sm: 6, md: 4, lg: 3 } as const;
    return <>

        {/* <Divider />
        <Typography variant="h4" margin={3}>
            OptiAnalytics
        </Typography> */}
        <Divider />
        <Box margin={0.5} paddingLeft={3} >
            <Breadcrumbs aria-label="breadcrumb">
                <Link underline="hover" color="inherit" href="/">
                    Home
                </Link>
                <Typography sx={{ color: 'text.primary' }}>Glossary</Typography>
            </Breadcrumbs>

        </Box>
        <Divider />
        {/*Search*/}
        <Grid container spacing={2} justifyContent={"space-between"}
            marginX={3} marginY={2}>
            <Grid size={{ xs: 12, md: 7, lg: 7, }}>
                <Box display={'flex'} flexDirection={'row'} alignContent={'center'}>
                    <TextField size="medium" label="Search" variant="outlined" fullWidth />

                </Box>
            </Grid>
            <Grid spacing={1} aria-label='Basic button group'
                size={{ xs: 12, md: 3, lg: 3 }}
            >
                <Stack spacing={1} direction={'row'} aria-label="Button group" >
                    <Button variant="contained" sx={{
                        borderRadius: '8px'
                    }}>Search</Button>

                    <Button variant="outlined" sx={{
                        borderRadius: '8px'
                    }}>Cancel</Button>
                </Stack>

            </Grid>

            <Grid size={{ xs: 12, md: 2, lg: 2 }} sx={{ display: 'flex', alignItems: 'left' }}>
                <Box sx={{ md: { ml: 3 } }}>
                    <Stack direction="row" spacing={1}>
                        <Link
                            component="button"
                            onClick={handleAddGlossary}
                            underline="none"
                            aria-label="Add Glossary Term"
                            sx={{
                                ":hover": { textDecoration: 'underline' }
                            }}
                        >
                            Add Glossary Term
                        </Link>
                        <Icon color="primary">add_circle</Icon>
                    </Stack>
                </Box>

            </Grid>
        </Grid>
        {/*Pagination */}
        <Divider sx={{ marginX: '25px' }} />
        <Box margin={3}>
            <Grid container>
                <Grid size={{
                    xs: 12, md: 12, lg: 2
                }}>
                    <Typography variant="h6" fontWeight={'bold'}>
                        Search Result
                    </Typography>
                </Grid>
                <Grid size={{
                    xs: 12, md: 12, lg: 10
                }} >
                    <Typography>

                        <Stack
                            alignItems="center"
                            flexDirection="row"
                            sx={{
                                flexGrow: 1, // If you want the stack to expand.
                                justifyContent: { lg: 'end' },
                            }}
                        >
                            1 to 50 of 111 Entries |
                            <ButtonGroup disableElevation>
                                <IconButton size="small">
                                    <SkipPreviousIcon fontSize="inherit" />
                                </IconButton>
                                <IconButton size="small">
                                    <ArrowBackIosIcon fontSize="inherit" />
                                </IconButton>
                            </ButtonGroup>


                            Page 1 of 3
                            <IconButton size="small">
                                <ArrowForwardIosIcon fontSize="inherit" />
                            </IconButton>
                            <IconButton size="small">
                                <SkipNextIcon fontSize="inherit" />
                            </IconButton>
                        </Stack>

                    </Typography>
                </Grid>
            </Grid>
        </Box>
        <Box
            margin={3} overflow={'auto'} minWidth={600}>
            <Grid container spacing={1}
                sx={(theme) => ({
                    '--Grid-borderWidth': '1px',
                    borderTop: 'var(--Grid-borderWidth) solid',
                    borderColor: 'divider',
                    '& > div': {
                        borderRight: 'var(--Grid-borderWidth) solid',
                        borderBottom: 'var(--Grid-borderWidth) solid',
                        borderColor: 'divider',
                        ...(Object.keys(colWidth) as Array<keyof typeof colWidth>).reduce(
                            (result, key) => ({
                                ...result,
                                [`&:nth-of-type(${12 / colWidth[key]}n)`]: {
                                    [theme.breakpoints.only(key)]: {
                                        borderRight: 'none',
                                    },
                                },
                            }),
                            {},
                        ),
                    },
                })}
            >
                {/*Header*/}
                <Grid size={{
                    sm: 2, md: 2, lg: 2,
                }} sx={{ padding: '5px' }}>
                    <Typography fontWeight={'bold'} variant="subtitle2" color="textSecondary">
                        Term Of Phrase</Typography>
                </Grid>
                <Grid size={{
                    sm: 9, md: 9, lg: 9,
                }} sx={{ padding: '5px' }}>
                    <Typography fontWeight={'bold'} variant="subtitle2" color="textSecondary">
                        Glossary Explaination
                    </Typography>
                </Grid>
                <Grid size={{
                    sm: 1, md: 1, lg: 1,
                }} sx={{ padding: '5px' }}>
                    <Typography fontWeight={'bold'} variant="subtitle2" color="textSecondary">
                        Action
                    </Typography>
                </Grid>
                <>
                    <Grid size={{
                        sm: 2, md: 2, lg: 2,
                    }}
                        sx={{
                            maxHeight: '120px',
                            padding: '7px'
                        }} >
                        <Typography variant="subtitle2" color="textSecondary">
                            Experience-Based Breakage</Typography>
                    </Grid>
                    <Grid size={{
                        sm: 9, md: 9, lg: 9,
                    }}
                        sx={{
                            maxHeight: '120px',
                            padding: '7px',
                            overflow: 'auto'
                        }} >
                        <Typography variant="subtitle2" color="textSecondary">
                            Experience-Based Breakage: A measure, taken on a monthly basis, of the actual breakage for the portfolio, computed as a fraction, with the numerator
                            being the total forecasted unredeemed amounts of all cards in the Card portfolio and the denominator being the total loads of the card portfolio. The
                            purpose of this measure is to provide a short-term red-flag metric regarding the actual breakage performance of the portfolio.puted quarterly, of the forecasted breakage versus the actual breakage, dawda
                        </Typography>
                    </Grid>
                    <Grid size={{
                        sm: 1, md: 1, lg: 1,
                    }}
                        alignContent={'center'} justifyContent={'center'}>

                        <IconButton aria-label="delete" size="large">
                            <CreateIcon />
                        </IconButton>
                    </Grid>
                </>
                <>
                    <Grid size={{
                        sm: 2, md: 2, lg: 2,
                    }}
                        sx={{
                            maxHeight: '120px',
                            padding: '7px'
                        }} >
                        <Typography variant="subtitle2" color="textSecondary">
                            Experience-Based Breakage</Typography>
                    </Grid>
                    <Grid size={{
                        sm: 9, md: 9, lg: 9,
                    }}
                        sx={{
                            maxHeight: '120px',
                            padding: '7px',
                            overflow: 'auto'
                        }} >
                        <Typography variant="subtitle2" color="textSecondary">
                            Experience-Based Breakage: A measure, taken on a monthly basis, of the actual breakage for the portfolio, computed as a fraction, with the numerator
                            being the total forecasted unredeemed amounts of all cards in the Card portfolio and the denominator being the total loads of the card portfolio. The
                            purpose of this measure is to provide a short-term red-flag metric regarding the actual breakage performance of the portfolio.puted quarterly, of the forecasted breakage versus the actual breakage, dawda
                        </Typography>
                    </Grid>
                    <Grid size={{
                        sm: 1, md: 1, lg: 1,
                    }}
                        alignContent={'center'} justifyContent={'center'}>

                        <IconButton aria-label="delete" size="large">
                            <CreateIcon />
                        </IconButton>
                    </Grid>
                </>
                <>
                    <Grid size={{
                        sm: 2, md: 2, lg: 2,
                    }}
                        sx={{
                            maxHeight: '120px',
                            padding: '7px'
                        }} >
                        <Typography variant="subtitle2" color="textSecondary">
                            Experience-Based Breakage</Typography>
                    </Grid>
                    <Grid size={{
                        sm: 9, md: 9, lg: 9,
                    }}
                        sx={{
                            maxHeight: '120px',
                            padding: '7px',
                            overflow: 'auto'
                        }} >
                        <Typography variant="subtitle2" color="textSecondary">
                            Experience-Based Breakage: A measure, taken on a monthly basis, of the actual breakage for the portfolio, computed as a fraction, with the numerator
                            being the total forecasted unredeemed amounts of all cards in the Card portfolio and the denominator being the total loads of the card portfolio. The
                            purpose of this measure is to provide a short-term red-flag metric regarding the actual breakage performance of the portfolio.puted quarterly, of the forecasted breakage versus the actual breakage, dawda
                        </Typography>
                    </Grid>
                    <Grid size={{
                        sm: 1, md: 1, lg: 1,
                    }}
                        alignContent={'center'} justifyContent={'center'}>

                        <IconButton aria-label="delete" size="large">
                            <CreateIcon />
                        </IconButton>
                    </Grid>
                </>
                <>
                    <Grid size={{
                        sm: 2, md: 2, lg: 2,
                    }}
                        sx={{
                            maxHeight: '120px',
                            padding: '7px'
                        }} >
                        <Typography variant="subtitle2" color="textSecondary">
                            Experience-Based Breakage</Typography>
                    </Grid>
                    <Grid size={{
                        sm: 9, md: 9, lg: 9,
                    }}
                        sx={{
                            maxHeight: '120px',
                            padding: '7px',
                            overflow: 'auto'
                        }} >
                        <Typography variant="subtitle2" color="textSecondary">
                            Experience-Based Breakage: A measure, taken on a monthly basis, of the actual breakage for the portfolio, computed as a fraction, with the numerator
                            being the total forecasted unredeemed amounts of all cards in the Card portfolio and the denominator being the total loads of the card portfolio. The
                            purpose of this measure is to provide a short-term red-flag metric regarding the actual breakage performance of the portfolio.puted quarterly, of the forecasted breakage versus the actual breakage, dawda
                        </Typography>
                    </Grid>
                    <Grid size={{
                        sm: 1, md: 1, lg: 1,
                    }}
                        alignContent={'center'} justifyContent={'center'}>

                        <IconButton aria-label="delete" size="large">
                            <CreateIcon />
                        </IconButton>
                    </Grid>
                </>
            </Grid>
        </Box>
    </>
}

const GlossaryPage: React.FC = () => {
    const idPage = Number(useLocation().pathname.replace('/glossary', ''));
    const activePage = useActivePage();
    const [isPageLoading, SetIsPageLoading] = React.useState<boolean>(true);

    useEffect(() => {
        const fetchRecordFromRange = async ({ }) => {

        }
    },)

    if (isPageLoading) {
        return <PageTemplate children={<></>} />
    }
    return <Box
        overflow={'auto'}
    >

    </Box>
}

export default GlossaryPage