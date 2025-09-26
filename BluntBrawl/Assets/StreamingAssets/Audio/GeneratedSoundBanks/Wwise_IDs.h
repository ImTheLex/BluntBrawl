/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID BUTTONCLICK = 4051332235U;
        static const AkUniqueID BUTTONFANCY = 911993200U;
        static const AkUniqueID BUTTONLAUNCH = 3389008624U;
        static const AkUniqueID BUTTONOVER = 927588791U;
        static const AkUniqueID ENEMYDASHCOOLDOWN = 4196958256U;
        static const AkUniqueID ENEMYEVENTSDASH = 3175741232U;
        static const AkUniqueID ENEMYEVENTSDIE = 723260870U;
        static const AkUniqueID ENEMYEVENTSFALL = 2172152223U;
        static const AkUniqueID ENEMYEVENTSFOOTSTEPS = 2772879681U;
        static const AkUniqueID ENEMYEVENTSSLASH = 3292113943U;
        static const AkUniqueID ENEMYEVENTSTOUCHED = 3200381734U;
        static const AkUniqueID ENEMYWEAPONCOOLDOWN = 1980102556U;
        static const AkUniqueID ENEMYWEAPONSEVENTHITS = 526621624U;
        static const AkUniqueID ENEMYWEAPONSEVENTSDROP = 1389296758U;
        static const AkUniqueID LOOSER = 598450249U;
        static const AkUniqueID MUSICSOUND = 2044738387U;
        static const AkUniqueID OBJECTEVENTSCATHAMMERONGROUND = 2963827865U;
        static const AkUniqueID OBJECTEVENTSCRATEHIT = 2354176855U;
        static const AkUniqueID OBJECTEVENTSFENCEHIT = 4263813725U;
        static const AkUniqueID OBJECTEVENTSHEAL = 2381628217U;
        static const AkUniqueID PLAYERDASHCOOLDOWN = 1317595701U;
        static const AkUniqueID PLAYEREVENTSDASH = 1505439485U;
        static const AkUniqueID PLAYEREVENTSDIE = 1082178505U;
        static const AkUniqueID PLAYEREVENTSFALL = 2509028246U;
        static const AkUniqueID PLAYEREVENTSFOOTSTEPS = 1744576738U;
        static const AkUniqueID PLAYEREVENTSSLASH = 2607198388U;
        static const AkUniqueID PLAYEREVENTSTOUCHED = 3327793297U;
        static const AkUniqueID PLAYERWEAPONCOOLDOWN = 25254557U;
        static const AkUniqueID PLAYERWEAPONSEVENTHITS = 4152903161U;
        static const AkUniqueID PLAYERWEAPONSEVENTSDROP = 1643626701U;
        static const AkUniqueID STARTCOUNT = 1190501004U;
        static const AkUniqueID TESTSYSTEM = 3425412358U;
        static const AkUniqueID TESTSYSTEMVR = 1251401278U;
        static const AkUniqueID TIMERENDBUZZ = 2134179288U;
        static const AkUniqueID TIMERTIC = 1964277448U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace MUSICSTATES
        {
            static const AkUniqueID GROUP = 3103015060U;

            namespace STATE
            {
                static const AkUniqueID INGAME = 984691642U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID WAINTING = 3862977292U;
            } // namespace STATE
        } // namespace MUSICSTATES

    } // namespace STATES

    namespace SWITCHES
    {
        namespace MUSICSWITCH
        {
            static const AkUniqueID GROUP = 1445037870U;

            namespace SWITCH
            {
                static const AkUniqueID INGAME = 984691642U;
                static const AkUniqueID WAITING = 19135406U;
            } // namespace SWITCH
        } // namespace MUSICSWITCH

        namespace SWITCHEWEAPONSTYPE
        {
            static const AkUniqueID GROUP = 1861379221U;

            namespace SWITCH
            {
                static const AkUniqueID SWITCHEWEAPONSTYPEBAT = 3307155126U;
                static const AkUniqueID SWITCHEWEAPONSTYPECATHAMMER = 3771686185U;
                static const AkUniqueID SWITCHEWEAPONSTYPEPIPE = 3160374033U;
                static const AkUniqueID SWITCHEWEAPONSTYPESIGN = 1444194542U;
            } // namespace SWITCH
        } // namespace SWITCHEWEAPONSTYPE

        namespace SWITCHHIT
        {
            static const AkUniqueID GROUP = 4139584672U;

            namespace SWITCH
            {
                static const AkUniqueID FULLHP = 2942549062U;
                static const AkUniqueID LOWHP = 624013381U;
                static const AkUniqueID MEDIUMHP = 2639122072U;
            } // namespace SWITCH
        } // namespace SWITCHHIT

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID AUDIOBUS = 1445131385U;
        static const AkUniqueID RTPCGAMETIMER = 847161145U;
        static const AkUniqueID RTPCPLAYERVOICEPERHEALTH = 1693736946U;
        static const AkUniqueID RTPCSLASHSOUNDPERVELOCITY = 3236278648U;
        static const AkUniqueID RTPCWEAPONINHAND = 147779934U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID BLUNTBRAWLSOUNDBANKS = 3600579960U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTERVRAUDIOBUS = 4032808771U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID MASTERVRAUXBUS = 1797866611U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
        static const AkUniqueID SYSTEMVR = 1998438890U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
