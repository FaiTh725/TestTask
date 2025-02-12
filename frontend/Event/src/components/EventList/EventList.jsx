import { useState } from "react";
import styles from "./EventList.module.css"
import { useEffect } from "react";
import axios from "axios";
import CenterVert from "../layots/CenterVert/CenterVert";
import EventCart from "../EventCart/EventCart";

const EventList = () => {
  const size = 12;
  const [page, setPage] = useState(1);
  const [events, setEvents] = useState([]);

  const handleGetEvents = async (abortSignal) => {
    try
    {
      var response = await axios.get(`https://localhost:7178/api/Event/GetEventsPagination?page=${page}&size=${size}`,
        {
          headers:{
            "Content-Type": "application/json"
          },
          withCredentials: true,
          signal: abortSignal
        }
      );
    
      if(response.data.statusCode === 200)
      { 
        setEvents([...response.data.data]);
      }
      else
      {
        console.error("Error Get All Events - " + response.data.description);
      }
    }
    catch
    {
      console.error("Error Get All Events");
    }
  }

  useEffect(() => {
    const abortController = new AbortController();
    const signal = abortController.signal;
    const executeGetEvents = async () => {
      await handleGetEvents(signal);
    }

    executeGetEvents();

    return () => {
      abortController.abort();
    }
  }, []);

  return (
    <div className={styles.EventList__Main}>
      <CenterVert>
        <div className={styles.EventList__Events}>
          {
            events.map(event => 
            <EventCart key={event.id} event={event}/>)
          }
        </div>
        <div className={styles.EventList__Pagination}>

        </div>
      </CenterVert>
    </div>
  )
}

export default EventList;