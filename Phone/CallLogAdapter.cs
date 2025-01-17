using Android.Content;
using Android.Graphics;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Phone
{
    public class CallLogAdapter : RecyclerView.Adapter
    {
        private readonly List<CallLogItem> _callLogItems;
        private readonly Context _context;

        public CallLogAdapter(List<CallLogItem> callLogItems, Context context)
        {
            _callLogItems = callLogItems;
            _context = context;
        }

        public override int ItemCount => _callLogItems.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is CallLogViewHolder vh)
            {
                var log = _callLogItems[position];
                vh.Name.Text = log.SubscribersName;
                vh.Number.Text = log.PhoneNumber;
                vh.Details.Text = $"{log.CallType}, {log.CallDate}, {log.CallDuration} сек.";

                Task.Run(() =>
                {
                    Bitmap bitmap = null;
                    string key = $"Bitmap:{log.PhoneNumber}";
                    bitmap = ImageCache.GetBitmap(key);

                    if (bitmap == null && log.Photo != null)
                    {
                        bitmap = BitmapFactory.DecodeByteArray(log.Photo, 0, log.Photo.Length);
                        ImageCache.AddBitmap(key, bitmap);
                    }

                    if (bitmap == null)
                    {
                        bitmap = ImageUtils.GetRoundedBitmap(vh.Avatar.Context, Resource.Drawable.ic_default_avatar);
                        ImageCache.AddBitmap(key, bitmap);
                    }

                    if (bitmap != null)
                    {
                        Bitmap roundedBitmap = ImageUtils.GetRoundedBitmap(bitmap);
                        if (_context is Activity activity)
                        {
                            activity.RunOnUiThread(() => vh.Avatar.SetImageBitmap(roundedBitmap));
                        }
                    }
                    else
                    {
                        if (_context is Activity activity)
                        {
                            activity.RunOnUiThread(() => vh.Avatar.SetImageResource(Resource.Drawable.ic_default_avatar));
                        }
                    }
                });
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.call_log_item, parent, false);
            return new CallLogViewHolder(itemView);
        }

    }

    public class CallLogViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; }
        public TextView Number { get; }
        public TextView Details { get; }
        public ImageView Avatar { get; }

        public CallLogViewHolder(View itemView) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.tvName);
            Number = itemView.FindViewById<TextView>(Resource.Id.tvNumber);
            Details = itemView.FindViewById<TextView>(Resource.Id.tvDetails);
            Avatar = itemView.FindViewById<ImageView>(Resource.Id.ivAvatar);
        }
    }
}
