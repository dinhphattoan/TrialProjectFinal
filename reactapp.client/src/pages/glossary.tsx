import React from "react";
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import {
    Box, Button, ButtonGroup, Card, CardContent, CardMedia, CircularProgress, Divider, Icon,
    IconButton, Modal, Skeleton, Stack, TextField, Typography
} from "@mui/material";
import Grid from '@mui/material/Grid2';
import CreateIcon from '@mui/icons-material/Create';
import SkipPreviousIcon from '@mui/icons-material/SkipPrevious';
import SkipNextIcon from '@mui/icons-material/SkipNext';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import { GetResponseServerAPI, PostResponseServerAPI } from "../validation";
import homeLogoImage from '../assets/mainlogo.png';
import ExceptionReportBox from "../components/ExceptionReportBox";
interface GlossaryItem {
    id: string;
    name: string;
    definition: string;
    createDate: string;
    lastModifyDate: string;
}
interface GlossaryRecords {
    currentPage: number;
    pageItems: any[];
    pageSize: number;
    totalItems: number;
    totalPages: number;
}
interface ModalCreateResult{
    termInput: boolean;
    explainationInput: boolean;
    submitResult: boolean;
    labelMessage: string;
}
const modalStyle = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 500,
};


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
const GlossaryItemComponent: React.FC<GlossaryItem> = ({ name, definition }) => {
    return (
        <>
            <Grid size={{
                xs: 12, sm: 2, md: 2, lg: 2,
            }}
                sx={{
                    padding: '7px'
                }} >
                <Typography variant="subtitle2" color="textSecondary">
                    {name}</Typography>
            </Grid>
            <Grid size={{
                xs: 12, sm: 9, md: 9, lg: 9,
            }}
                sx={{
                    padding: '7px',
                    overflow: 'auto'
                }} >
                <Typography variant="subtitle2" color="textSecondary">
                    {definition}
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
                    key={item.id}
                    id={item.id}
                    name={item.name}
                    definition={item.definition}
                    createDate={item.createDate}
                    lastModifyDate={item.lastModifyDate}
                />
            ))}
        </>
    );
};

const GlossaryPage: React.FC = () => {
    const defaultGlossaryRecords: GlossaryRecords = {
        currentPage: 1,
        pageItems: [],
        pageSize: 10,
        totalItems: 0,
        totalPages: 0
    }
    const defaultModalCreateResult: ModalCreateResult={
        termInput: true,
        explainationInput: true,
        submitResult: false,
        labelMessage: ""
    }
    const [glossaryRecords, SetGlossaryRecords] = React.useState<GlossaryRecords>(defaultGlossaryRecords);
    const [isDataFetching, SetIsDataFetching] = React.useState<boolean>(true);

    const [emptyGlossaryRecordInfo, SetEmptyGlossaryRecordInfo] = React.useState<string>("No Glossary Record Found");
    const [isSeaching, SetIsSearching] = React.useState<boolean>(false);
    const [searchValue, SetSearchValue] = React.useState<string>("");
    const [searchMode, SetSearchMode] = React.useState<boolean>(false);
    const [searchResultValue, SetSearchResultValue] = React.useState<string>("");

    const [openModel, SetOpenModel] = React.useState<boolean>(false);
    const [modalSubmitCreateResult, SetModalSubmitCreateResult] = React.useState<ModalCreateResult>(defaultModalCreateResult);
    const [inputAddTermOfPhrase, SetInputAddTermOfPhraseValue] = React.useState<string>("");
    const [inputAddGlossaryExplainationValue, SetInputAddGlossaryExplainationValue] = React.useState<string>("");
    const [isInCreatingGlossary, SetIsInCreatingGlossary] = React.useState<boolean>(false);


    const AddGlossaryModalOpen = () => SetOpenModel(true);
    const CloseAddGlossaryModal = () => {
        SetOpenModel(false);
        SetInputAddTermOfPhraseValue("");
        SetModalSubmitCreateResult(defaultModalCreateResult);
        SetInputAddGlossaryExplainationValue("");
        // fetchGlossaryRecords(defaultGlossaryRecords);
    };

    const fetchGlossaryRecords = async (records: GlossaryRecords) => {
        SetIsDataFetching(true);
        SetIsSearching(true);
        try {
            SetSearchResultValue("");
            let record_URL_BASE = `/api/Glossaries?`;
            if (searchMode && searchValue.length !== 0) {
                record_URL_BASE += `Keyword=${searchValue}&`
            }
            record_URL_BASE += `OrderBy=TermOfPhrase`;
            record_URL_BASE += '&Desc=false';
            record_URL_BASE += `&Page=${records?.currentPage}`
            record_URL_BASE += `&PageSize=${records?.pageSize}`
            const response = await GetResponseServerAPI(record_URL_BASE);

            if (!response.ok) {
                SetGlossaryRecords(defaultGlossaryRecords);
                SetEmptyGlossaryRecordInfo("Error retrieves Glossary Record.");
                return;
            }
            const dataJson = await response.json();
            SetGlossaryRecords(dataJson);
            SetEmptyGlossaryRecordInfo("No Glossary found!");
        }
        catch (error) {
            console.error("Error retrieve Glossary Record.", error)
            SetGlossaryRecords(defaultGlossaryRecords);
            SetEmptyGlossaryRecordInfo("Error retrieves Glossary Record.");
        }
        finally {
            SetIsSearching(false);
            SetIsDataFetching(false);
        }

    }
    const createGlossaryTerm = async () => {
        try {
            SetModalSubmitCreateResult(defaultModalCreateResult);
            SetIsInCreatingGlossary(true);

            const response = await PostResponseServerAPI("/api/Glossaries",
                { termOfPhrase: inputAddTermOfPhrase, explaination: inputAddGlossaryExplainationValue });
            if (!response.ok) {
                CloseAddGlossaryModal();
                fetchGlossaryRecords(defaultGlossaryRecords);
            }
            const data = await response.json();
            if (!data.success) {
                SetModalSubmitCreateResult({termInput:false, explainationInput: true ,submitResult: false, labelMessage: "Term are already exist." });
                return;
            }
            SetModalSubmitCreateResult({ termInput:true, explainationInput: true ,submitResult: true, labelMessage: "Successfully Create Glossary Record." });
        }
        catch (error) {
            console.error("Error Create Glossary Record.", error);
            SetModalSubmitCreateResult({ termInput:false, explainationInput: false ,submitResult: false, labelMessage: "Error Create Glossary Record." });
        }
        finally {
            SetIsInCreatingGlossary(false);
        }
    }

    React.useEffect(() => {
        fetchGlossaryRecords(defaultGlossaryRecords);
    }, [searchMode])

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
                                SetGlossaryRecords(defaultGlossaryRecords)
                                if (searchMode) {
                                    fetchGlossaryRecords(defaultGlossaryRecords);
                                    return;
                                }
                                SetSearchMode(true);
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
                                SetSearchValue("");
                                SetSearchMode(false);
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

                                {((glossaryRecords.currentPage - 1) * glossaryRecords.pageSize) + 1}  to {((glossaryRecords.currentPage - 1) * glossaryRecords.pageSize) + glossaryRecords.pageItems.length} of {glossaryRecords.totalItems} Entries |

                            </Typography>
                            <ButtonGroup disableElevation>
                                <IconButton
                                    onClick={async () => {
                                        await fetchGlossaryRecords({
                                            ...defaultGlossaryRecords,
                                            currentPage: 1,
                                        });
                                    }}
                                    disabled={glossaryRecords.currentPage === 1 || isDataFetching}
                                    size="small"
                                    aria-label="Go to first page"
                                >
                                    <SkipPreviousIcon fontSize="inherit" />
                                </IconButton>
                                <IconButton
                                    onClick={async () => {
                                        await fetchGlossaryRecords({
                                            ...defaultGlossaryRecords,
                                            currentPage: glossaryRecords.currentPage - 1
                                        });
                                    }}
                                    disabled={glossaryRecords.currentPage === 1 || isDataFetching}
                                    size="small"
                                    aria-label="Go to previous page"
                                >
                                    <ArrowBackIosIcon fontSize="inherit" />
                                </IconButton>
                            </ButtonGroup>
                            Page {glossaryRecords.currentPage} of {glossaryRecords.totalPages}
                            <IconButton
                                onClick={async () => {
                                    await fetchGlossaryRecords({
                                        ...defaultGlossaryRecords,
                                        currentPage: glossaryRecords.currentPage + 1
                                        
                                    });
                                }}
                                disabled={glossaryRecords.currentPage == glossaryRecords.totalPages ||
                                    isDataFetching}
                                size="small"
                                aria-label="Go to next page"
                            >
                                <ArrowForwardIosIcon fontSize="inherit" />
                            </IconButton>
                            <IconButton
                                onClick={async () => {
                                    await fetchGlossaryRecords({
                                        ...defaultGlossaryRecords,
                                        currentPage: glossaryRecords.totalPages
                                    });
                                }}
                                disabled={glossaryRecords.currentPage == glossaryRecords.totalPages ||
                                    isDataFetching}
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
                            glossaryRecords.totalItems == 0 ? <ExceptionReportBox infoMessage={emptyGlossaryRecordInfo} /> :
                                <GlossaryGridRecord glossaryItems={glossaryRecords.pageItems} />

                    }
                </Grid>

            </Box>

        </>
        <Modal
            keepMounted
            open={openModel}
            onClose={CloseAddGlossaryModal}
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
                                        || (!modalSubmitCreateResult?.termInput)
                                    }
                                />
                                <TextField multiline
                                    helperText={`${inputAddGlossaryExplainationValue.length}/500`}
                                    onChange={(e) => {
                                        SetInputAddGlossaryExplainationValue(e.target.value);
                                    }}
                                    error={inputAddGlossaryExplainationValue.length > 500
                                        || (!modalSubmitCreateResult?.explainationInput)
                                    }
                                    inputMode="text"
                                    label="Glossary Explaination *"
                                    aria-label="Glossary Explaination Input"
                                    sx={{ "& .MuiInputBase-input": { minHeight: "150px" } }}
                                    value={inputAddGlossaryExplainationValue}
                                />


                            </Stack>
                            <Divider />
                            <>
                                <Stack spacing={2} marginTop={2} direction={'row-reverse'}>
                                    <Button onClick={
                                        createGlossaryTerm
                                    } disabled={
                                        !modalSubmitCreateResult?.submitResult? (inputAddTermOfPhrase.length === 0 || inputAddGlossaryExplainationValue.length === 0) : true
                                    } size="medium" variant="contained" color="primary" sx={{ borderRadius: '20px' }} >
                                        {isInCreatingGlossary ? <CircularProgress size={'25px'} /> :
                                            'Submit'}
                                    </Button>
                                    
                                    <Button size="medium" onClick={CloseAddGlossaryModal} variant="contained" color="secondary" sx={{ borderRadius: '20px' }}>Cancel</Button>
                                </Stack>
                                {modalSubmitCreateResult && (
                                    <Typography color={modalSubmitCreateResult.submitResult ? 'success' : 'error'}
                                        textAlign={'end'} paddingRight={1} marginTop={1} fontSize={"12px"}>{modalSubmitCreateResult.labelMessage}</Typography>
                                )}
                            </>

                        </Box>
                    </CardContent>
                </Card>
            </Box>
        </Modal>
    </Box>
}

export default GlossaryPage