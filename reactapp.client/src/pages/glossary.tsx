import React from "react";
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import { Box, Button, ButtonGroup, Card, CardContent, CardMedia, CircularProgress, Divider, Icon, IconButton, Modal, Paper, Skeleton, Stack, TextField, Typography } from "@mui/material";
import Grid from '@mui/material/Grid2';
import CreateIcon from '@mui/icons-material/Create';
import SkipPreviousIcon from '@mui/icons-material/SkipPrevious';
import SkipNextIcon from '@mui/icons-material/SkipNext';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import { GetResponseServerAPI, PostResponseServerAPI } from "../validation";
import homeLogoImage from '../assets/mainlogo.png';
interface GlossaryItem {
    guid: string;
    termOfPhrase: string;
    glossaryExplaination: string;
    dateAdded: string;
    userCreatedBy: null;
}

const modalStyle = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 500,
};

const EmptyGlossariesComponent: React.FC<{ infoMessage: string }> = ({ infoMessage }) => {
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
const SkeletonTemplate: React.FC = () => {
    return <>
        <Grid size={{
            sm: 2, md: 2, lg: 2,
        }}
            sx={{
                padding: '7px'
            }} >
            <Skeleton animation="wave" height={100} />
        </Grid>
        <Grid size={{
            sm: 9, md: 9, lg: 9,
        }}
            sx={{
                padding: '7px',
                overflow: 'auto'
            }} >
            <Skeleton animation="wave" height={100} />
        </Grid>
        <Grid size={{
            sm: 1, md: 1, lg: 1,
        }}
            alignContent={'center'} justifyContent={'center'}>

            <Skeleton animation="wave" height={100} />
        </Grid>
        <Grid size={{
            sm: 2, md: 2, lg: 2,
        }}
            sx={{
                padding: '7px'
            }} >
            <Skeleton animation="wave" height={100} />
        </Grid>
        <Grid size={{
            sm: 9, md: 9, lg: 9,
        }}
            sx={{
                padding: '7px',
                overflow: 'auto'
            }} >
            <Skeleton animation="wave" height={100} />
        </Grid>
        <Grid size={{
            sm: 1, md: 1, lg: 1,
        }}
            alignContent={'center'} justifyContent={'center'}>

            <Skeleton animation="wave" height={100} />
        </Grid>
    </>


}
const GlossaryItemComponent: React.FC<GlossaryItem> = ({ termOfPhrase, glossaryExplaination }) => {
    return (
        <>
            <Grid size={{
                xs: 12, sm: 2, md: 2, lg: 2,
            }}
                sx={{
                    padding: '7px'
                }} >
                <Typography variant="subtitle2" color="textSecondary">
                    {termOfPhrase}</Typography>
            </Grid>
            <Grid size={{
                xs: 12, sm: 9, md: 9, lg: 9,
            }}
                sx={{
                    padding: '7px',
                    overflow: 'auto'
                }} >
                <Typography variant="subtitle2" color="textSecondary">
                    {glossaryExplaination}
                </Typography>
            </Grid>
            <Grid size={{
                xs: 12, sm: 1, md: 1, lg: 1,
            }}
                alignContent={'center'} justifyContent={'center'}>

                <IconButton aria-label="delete" size="large">
                    <CreateIcon />
                </IconButton>
            </Grid>
        </>
    )
}
const GlossaryGridRecord: React.FC<{ glossaryItems: GlossaryItem[] }> = ({ glossaryItems }) => {
    return (
        <>
            {glossaryItems.map(item => (
                <GlossaryItemComponent
                    key={item.guid}
                    guid={item.guid}
                    termOfPhrase={item.termOfPhrase}
                    glossaryExplaination={item.glossaryExplaination}
                    dateAdded={item.dateAdded}
                    userCreatedBy={item.userCreatedBy}
                />
            ))}
        </>
    );
};

const GlossaryPage: React.FC = () => {

    const [glossaryRecords, SetGlossaryRecords] = React.useState<GlossaryItem[]>([]);
    const [isDataFetching, SetIsDataFetching] = React.useState<boolean>(true);
    const [pageIndex, SetPageIndex] = React.useState<number>(0);
    const [totalRecord, SetTotalRecord] = React.useState<number>(0);
    const [totalPageRecord, SetTotalPageRecord] = React.useState<number>(1);
    const [emptyGlossaryRecordInfo, SetEmptyGlossaryRecordInfo] = React.useState<string>("No Glossary Record Found");
    const [isSeaching, SetIsSearching] = React.useState<boolean>(false);
    const [searchValue, SetSearchValue] = React.useState<string>("");
    const [searchMode, SetSearchMode] = React.useState<boolean>(false);
    const [searchResultValue, SetSearchResultValue] = React.useState<string>("");

    const [openModel, SetOpenModel] = React.useState<boolean>(false);
    const [modalSubmitCreateResult, SetModalSubmitCreateResult] = React.useState<{submitResult: boolean, labelMessage: string} | null>(null);
    const [inputAddTermOfPhrase, SetInputAddTermOfPhraseValue] = React.useState<string>("");
    const [inputAddGlossaryExplainationValue, SetInputAddGlossaryExplainationValue] = React.useState<string>("");
    const [isInCreatingGlossary,SetIsInCreatingGlossary] = React.useState<boolean>(false);
    

    const AddGlossaryModalOpen = () => SetOpenModel(true);
    const AddGlossaryModalClose = () => {
        SetOpenModel(false);
        SetInputAddTermOfPhraseValue("");
        SetInputAddGlossaryExplainationValue("");
    };

    const retrieveGlossaryRecords = async (searchMode: boolean) => {
        SetIsDataFetching(true);
        SetIsSearching(true);
        try {

            let totalCount_URL_BASE: string = "/api/Glossaries/totalcount";
            if (searchMode) {

                totalCount_URL_BASE = `/api/Glossaries/search/total?value=${searchValue}`;
            }
            SetSearchResultValue("");

            const responseTotalCount = await GetResponseServerAPI(totalCount_URL_BASE);

            const dataRecordCount = await responseTotalCount.json();
            SetTotalRecord(dataRecordCount);

            SetTotalPageRecord(Math.ceil(dataRecordCount / totalRecordPerPage));

            let record_URL_BASE = `/api/Glossaries/${pageIndex * totalRecordPerPage}/${totalRecordPerPage}`;
            if (searchMode) {
                record_URL_BASE = `/api/Glossaries/search/index?value=${searchValue}&index=${pageIndex}&count=${totalRecordPerPage}`;
            }

            const recordResponse = await GetResponseServerAPI(record_URL_BASE);
            const recordJson: GlossaryItem[] = await recordResponse.json();
            SetEmptyGlossaryRecordInfo("No Glossary found!");

            SetGlossaryRecords(recordJson);
        }
        catch (error) {
            console.error("Error retrieve Glossary Record.", error)
            SetGlossaryRecords([]);
            SetEmptyGlossaryRecordInfo("Error retrieves Glossary Record.");
        }
        finally {
            SetIsSearching(false);
            SetIsDataFetching(false);
        }

    }
    const handleAddGlossaryTerm = async () => {
        try {
            SetModalSubmitCreateResult(null);
            SetIsInCreatingGlossary(true);
            const response = await PostResponseServerAPI("/api/Glossaries/add",
                { termOfPhrase: inputAddTermOfPhrase, "explaination": inputAddGlossaryExplainationValue });
            if (!response.ok) {
                AddGlossaryModalClose();
                retrieveGlossaryRecords(false);
            }
            SetModalSubmitCreateResult({submitResult: true, labelMessage: "Successfully Create Glossary Record."});
        }
        catch (error) {
            console.error("Error Create Glossary Record.", error);
            SetModalSubmitCreateResult({submitResult: false, labelMessage: "Error Create Glossary Record."});
        }
        finally{
            SetIsInCreatingGlossary(false);
        }
    }
    let totalRecordPerPage: number = 8;
    const toRange = (pageIndex * totalRecordPerPage + totalRecordPerPage);
    const displayedToRange = toRange > totalRecord ? totalRecord : toRange;

    React.useEffect(() => {
        retrieveGlossaryRecords(false);
    }, [])

    const colWidth = { xs: 12, sm: 6, md: 4, lg: 3 } as const;

    return <Box
        overflow={'auto'}
    >

        <>
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
                        <TextField id="searchInputID" size="medium" onChange={(e) => {
                            SetSearchValue(e.target.value);
                        }} label="Search" variant="outlined" fullWidth
                            error={searchResultValue.length !== 0}
                            helperText={searchResultValue}
                            value={searchValue}
                        />
                    </Box>
                </Grid>
                <Grid spacing={1} aria-label='Basic button group'
                    size={{ xs: 12, md: 3, lg: 3 }}
                >
                    <Stack spacing={1} direction={'row'} aria-label="Button group"
                        alignItems={'center'}>
                        <Button size={'small'} variant="contained" sx={{
                            borderRadius: '8px', padding: '5px'
                        }}
                            onClick={async () => {
                                if (searchValue.trim().length == 0) {
                                    SetSearchResultValue("Invalid search input");
                                    return;
                                }
                                SetPageIndex(0);
                                SetSearchMode(true);
                                await retrieveGlossaryRecords(true);
                            }}
                            disabled={isSeaching}
                        >
                            {isSeaching ? <CircularProgress size="25px" /> :
                                'Search'}
                        </Button>

                        <Button size={'small'} variant="outlined" sx={{
                            borderRadius: '8px', padding: '5px'
                        }}
                            disabled={!searchMode}
                            onClick={async () => {
                                SetSearchMode(false);
                                SetSearchValue("");
                                await retrieveGlossaryRecords(false);
                            }}
                        >Cancel</Button>
                    </Stack>

                </Grid>

                <Grid size={{ xs: 12, md: 2, lg: 2 }} sx={{ display: 'flex', alignItems: 'left' }}>
                    <Box sx={{ md: { ml: 3 } }}>
                        <Stack direction="row" spacing={1} alignItems={'center'}>
                            <Link
                                component="button"
                                onClick={AddGlossaryModalOpen}
                                underline="none"
                                aria-label="Add Glossary Term"

                                sx={{
                                    ":hover": { textDecoration: 'underline' },
                                    fontSize: '15px'
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
                            {searchMode ? 'Search Result' : ''}
                        </Typography>
                    </Grid>
                    <Grid size={{
                        xs: 12, md: 12, lg: 10
                    }} >
                        <Stack
                            alignItems="center"
                            flexDirection="row"
                            sx={{ flexGrow: 1, justifyContent: { lg: 'end' } }}
                        >
                            <Typography>
                                {pageIndex * totalRecordPerPage + 1} to {displayedToRange} of {totalRecord} Entries |
                            </Typography>
                            <ButtonGroup disableElevation>
                                <IconButton
                                    onClick={() => {
                                        SetPageIndex(0);
                                        retrieveGlossaryRecords(false);
                                    }}
                                    disabled={pageIndex === 0}
                                    size="small"
                                    aria-label="Go to first page"
                                >
                                    <SkipPreviousIcon fontSize="inherit" />
                                </IconButton>
                                <IconButton
                                    onClick={() => {
                                        SetPageIndex(pageIndex > 0 ? pageIndex - 1 : pageIndex);
                                        retrieveGlossaryRecords(false);
                                    }}
                                    disabled={pageIndex === 0}
                                    size="small"
                                    aria-label="Go to previous page"
                                >
                                    <ArrowBackIosIcon fontSize="inherit" />
                                </IconButton>
                            </ButtonGroup>
                            Page {pageIndex + 1} of {totalPageRecord}
                            <IconButton
                                onClick={() => {
                                    SetPageIndex(pageIndex < totalPageRecord - 1 ? pageIndex + 1 : pageIndex)
                                    retrieveGlossaryRecords(false);
                                }}
                                disabled={totalRecordPerPage * (pageIndex + 1) >= totalRecord}
                                size="small"
                                aria-label="Go to next page"
                            >
                                <ArrowForwardIosIcon fontSize="inherit" />
                            </IconButton>
                            <IconButton
                                onClick={() => {
                                    SetPageIndex(totalPageRecord - 1)
                                    retrieveGlossaryRecords(false);
                                }
                                }
                                disabled={totalRecordPerPage * (pageIndex + 1) >= totalRecord}
                                size="small"
                                aria-label="Go to last page"
                            >
                                <SkipNextIcon fontSize="inherit" />
                            </IconButton>
                        </Stack>

                    </Grid>
                </Grid>
            </Box>

            <Box
                margin={3} >
                <Grid container spacing={1} size={12}
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
                    <Grid size={{
                        xs: 12,
                        sm: 2, md: 2, lg: 2,
                    }} sx={{ padding: '5px' }}>
                        <Typography fontWeight={'bold'} variant="subtitle2" color="textSecondary">
                            Term Of Phrase</Typography>
                    </Grid>
                    <Grid size={{
                        xs: 12,
                        sm: 9, md: 9, lg: 9,
                    }} sx={{ padding: '5px' }}>
                        <Typography fontWeight={'bold'} variant="subtitle2" color="textSecondary">
                            Glossary Explaination
                        </Typography>
                    </Grid>
                    <Grid size={{
                        xs: 12,
                        sm: 1, md: 1, lg: 1,
                    }} sx={{ padding: '5px' }}>
                        <Typography fontWeight={'bold'} variant="subtitle2" color="textSecondary">
                            Action
                        </Typography>
                    </Grid>
                    {/*====================*/}
                    {
                        isDataFetching ? <SkeletonTemplate /> :
                            glossaryRecords.length == 0 ? <EmptyGlossariesComponent infoMessage={emptyGlossaryRecordInfo} /> :
                                <GlossaryGridRecord glossaryItems={glossaryRecords} />

                    }
                </Grid>

            </Box>

        </>
        <Modal
            keepMounted
            open={openModel}
            onClose={AddGlossaryModalClose}
            aria-labelledby="keep-mounted-modal-title"
            aria-describedby="keep-mounted-modal-description"
            hideBackdrop
        >
            <Box sx={modalStyle}>
                <Card elevation={10}>
                    <CardMedia
                        image={homeLogoImage}
                        component="img"
                        alt="My Logo"
                        sx={{
                            height: 70,
                            width: 'auto',
                            objectFit: 'fill',
                            marginLeft: '20px',
                            bgcolor: 'primary'
                        }}

                    />
                    <Divider />
                    <CardContent>
                        <Box marginX={3}>
                            <Box >
                                <Typography variant="h6" margin={2}>
                                    Add Glossary Term or Phrase
                                </Typography>
                            </Box>
                            <Divider />
                            <Stack marginTop={3} spacing={3}>
                                <TextField label="Term or Phrase *"
                                    helperText={`${inputAddTermOfPhrase.length}/50`}
                                    onChange={(e) => {
                                        SetInputAddTermOfPhraseValue(e.target.value);
                                    }}
                                    value={inputAddTermOfPhrase}
                                    error={inputAddTermOfPhrase.length > 50
                                        || (modalSubmitCreateResult !==null && !modalSubmitCreateResult.submitResult)
                                    }
                                />
                                <TextField multiline
                                    helperText={`${inputAddGlossaryExplainationValue.length}/500`}
                                    onChange={(e) => {
                                        SetInputAddGlossaryExplainationValue(e.target.value);
                                    }}
                                    error={inputAddGlossaryExplainationValue.length > 500
                                        || (modalSubmitCreateResult !==null && !modalSubmitCreateResult.submitResult)
                                    }
                                    inputMode="text"
                                    label="Glossary Explaination *"
                                    aria-label="Glossary Explaination Input"
                                    sx={{ "& .MuiInputBase-input": { minHeight: "150px" } }}
                                    value={inputAddGlossaryExplainationValue}
                                />


                            </Stack>
                            <Divider />
                            <Stack spacing={2} marginTop={2} direction={'row-reverse'}>
                            <Button onClick={
                                handleAddGlossaryTerm
                            } disabled={inputAddTermOfPhrase.length === 0 || inputAddGlossaryExplainationValue.length === 0} size="medium" variant="contained" color="primary" sx={{ borderRadius: '20px' }} >
                                {isInCreatingGlossary? <CircularProgress size={'25px'}/>:
                                'Submit'}
                                </Button>
                                <Button size="medium" onClick={AddGlossaryModalClose} variant="contained" color="secondary" sx={{ borderRadius: '20px' }}>Cancel</Button>
                            </Stack>
                            {modalSubmitCreateResult && (
                                <Typography color={modalSubmitCreateResult.submitResult ? 'success' : 'error'} 
                                textAlign={'end'} paddingRight={1} marginTop={1} fontSize={"12px"}>{modalSubmitCreateResult.labelMessage}</Typography>
                            )}
                        </Box>
                    </CardContent>
                </Card>
            </Box>
        </Modal>
    </Box>
}

export default GlossaryPage