const FormatDate = (date) => {

  const dateObj = new Date(date);

  if(isNaN(dateObj.getTime()))
  {
    return "Invalid Format Date"
  }

  const day = dateObj.getDate();
  const month = dateObj.toLocaleString('default', { month: 'short' });
  // const year = dateObj.getUTCFullYear();
  const hours = dateObj.getHours();
  const minues = dateObj.getMinutes();

  return `${hours}:${minues} ${day} ${month}`;
}

export default FormatDate;